Framework "4.6.1"

properties {
    $projectName = "FizzBuzz"
	$base_dir = resolve-path .\
	$source_dir = "$base_dir\src"
    $unitTestAssembly = "UnitTests.dll"
    $integrationTestAssembly = "IntegrationTests.dll"
    $acceptanceTestAssembly = "SmokeTests.dll"
    $acceptanceTestProject = "$source_dir\SmokeTests\SmokeTests.csproj"
	$projectConfig = $env:Configuration
    $version = $env:Version
    $nunitPath = Resolve-Path("$source_dir\packages\NUnit.Console*\Tools")

	$nunitExe = Join-Path $nunitPath nunit3-console.exe

    $specflowPath = Resolve-Path("$source_dir\packages\SpecFlow*\tools")
	$nunitoutput = "--result=$build_dir\TestResult.xml"
	
	# Psake support not yet officially available for MSBuild 15.0
	$msbuildExe = Resolve-Path("C:\Program Files (x86)\Microsoft Visual Studio\2017\*\MSBuild\15.0\Bin\msbuild")

	$build_dir = "$base_dir\build"
	$test_dir = "$build_dir\test"
	$testCopyIgnorePath = "_ReSharper"
	$package_dir = "$build_dir\package"	
	$package_file = "$build_dir\latestVersion\" + $projectName +"_Package.zip"
    $runOctoPack = $env:RunOctoPack

	$nupkgs = Join-Path $build_dir "*.nupkg"

    $databaseName = $projectName
    $databaseServer = "localhost\SQLEXPRESS2014"
    $databaseScripts = "$source_dir\Database\scripts"
    $hibernateConfig = "$source_dir\hibernate.cfg.xml"
    $schemaDatabaseName = $databaseName + "_schema"
    $integratedSecurity = "Integrated Security=true"
    
    $connection_string = "server=$databaseserver;database=$databasename;$databaseUser;"
    $AliaSql = "$source_dir\Database\scripts\AliaSql.exe"
    $webapp_dir = "$source_dir\UI"

	
    if(![string]::IsNullOrEmpty($env:APPVEYOR_BUILD_VERSION)) { $version = $env:APPVEYOR_BUILD_VERSION }
    if([string]::IsNullOrEmpty($version)) { $version = "1.0.0"}
    if([string]::IsNullOrEmpty($projectConfig)) {$projectConfig = "Release"}
    if([string]::IsNullOrEmpty($runOctoPack)) {$runOctoPack = "true"}
}

task default -depends Init, Compile, Test
task ci -depends CiInit, Init, CommonAssemblyInfo, Compile, Test, Publish

task CiInit {
	# AppVeyor uses a special build of nunit3-console for the AppVeyor format
	$script:nunitExe = "nunit3-console"
	$script:nunitoutput = "--result=$build_dir\TestResult.xml;format=AppVeyor"
}

task Init {
    delete_file $package_file
    delete_directory $build_dir
    create_directory $test_dir
    create_directory $build_dir

    #Write-Host $projectConfig
    #Write-Host $version
    #Write-Host $runOctoPack
}

task ConnectionString {
    $connection_string = "server=$databaseserver;database=$databasename;$integratedSecurity;"
    write-host "Using connection string: $connection_string"
    if ( Test-Path "$hibernateConfig" ) {
        poke-xml $hibernateConfig "//e:property[@name = 'connection.connection_string']" $connection_string @{"e" = "urn:nhibernate-configuration-2.2"}
    }
}

task Compile -depends Init {
	exec {
		#echo "$msbuildExe"
        & msbuild /t:Clean`;Rebuild /v:q /nologo /p:Configuration=$projectConfig /p:OctoPackPackageVersion=$version /p:RunOctoPack=$runOctoPack /p:OctoPackEnforceAddingFiles=true $source_dir\$projectName.sln
    }

	Copy_and_flatten $source_dir *.nupkg $build_dir
}

task Test -depends Compile {
    copy_all_assemblies_for_test $test_dir
    exec {
        & $nunitExe $test_dir\$unitTestAssembly $script:nunitoutput --workers=1 --noheader 
    }
}

task Publish -depends Test {
	Get-ChildItem $nupkgs | % { Push-AppveyorArtifact $_.FullName -FileName $_.Name }
}

task AcceptanceTest -depends Test {
    copy_all_assemblies_for_test $test_dir
	exec {
        & $nunitExe $test_dir\$acceptanceTestAssembly $script:nunitoutput --workers=1 --noheader --result="$build_dir\AcceptanceTestResult.xml"`;format=nunit2 --out="$build_dir\AcceptanceTestResult.txt"
        & $specflowPath\specflow.exe nunitexecutionreport $acceptanceTestProject /xmlTestResult:"$build_dir\AcceptanceTestResult.xml" /testOutput:"$build_dir\AcceptanceTestResult.txt" /out:"$build_dir\AcceptanceTestResult.html"
	}
}

task RebuildDatabase -depends ConnectionString {
    exec {
        & $AliaSql Rebuild $databaseServer $databaseName $databaseScripts
    }
}

task LoadData -depends ConnectionString, Compile, RebuildDatabase {
	exec { 
		& $nunitExe $test_dir\$integrationTestAssembly --where "cat == DataLoader" --noheader --result="$build_dir\DataLoadResult.xml"`;format=nunit3
    } "Build failed - data load failure"  
}

task CreateCompareSchema -depends SchemaConnectionString {
    exec {
        & $AliaSql Rebuild $databaseServer $schemaDatabaseName $databaseScripts
    }
}

task SchemaConnectionString {
    $connection_string = "server=$databaseserver;database=$schemaDatabaseName;@integratedSecurity;"
    write-host "Using connection string: $connection_string"
    #if ( Test-Path "$hibernateConfig" ) {
    #    poke-xml $hibernateConfig "//e:property[@name = 'connection.connection_string']" $connection_string @{"e" = "urn:nhibernate-configuration-2.2"}
    #}
}

task CommonAssemblyInfo {   
    create-commonAssemblyInfo "$version" $projectName "$source_dir\CommonAssemblyInfo.cs"
}
 
























function global:zip_directory($directory,$file) {
    write-host "Zipping folder: " $test_assembly
	write-host "Zipping directory: " $directory
	write-host "Zipping file: " $file
	write-host "Base: " $base_dir
	
	
    delete_file $file
    cd $directory
    & "$base_dir\tools\7zip\7z.exe" a -mx=9 -r $file
    cd $base_dir
}
function global:copy_website_files($source,$destination){
    $exclude = @('*.user','*.dtd','*.tt','*.cs','*.csproj','*.orig', '*.log') 
    copy_files $source $destination $exclude
	delete_directory "$destination\obj"
}

function global:copy_files($source,$destination,$exclude=@()){    
    create_directory $destination
    Get-ChildItem $source -Recurse -Exclude $exclude | Copy-Item -Destination {Join-Path $destination $_.FullName.Substring($source.length)} 
}

function global:Copy_and_flatten ($source,$filter,$dest) {
  ls $source -filter $filter  -r | Where-Object{!$_.FullName.Contains("$testCopyIgnorePath") -and !$_.FullName.Contains("packages") }| cp -dest $dest -force
}

function global:copy_all_assemblies_for_test($destination){
  create_directory $destination
  Copy_and_flatten $source_dir *.exe $destination
  Copy_and_flatten $source_dir *.dll $destination
  Copy_and_flatten $source_dir *.config $destination
  Copy_and_flatten $source_dir *.xml $destination
  Copy_and_flatten $source_dir *.pdb $destination
  Copy_and_flatten $source_dir *.sql $destination
  Copy_and_flatten $source_dir *.xlsx $destination
}

function global:delete_file($file) {
    if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null } 
}

function global:delete_directory($directory_name)
{
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:delete_files_in_dir($dir)
{
	get-childitem $dir -recurse | foreach ($_) {remove-item $_.fullname}
}

function global:create_directory($directory_name)
{
  mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}

function global:create-commonAssemblyInfo($version,$applicationName,$filename)
{
"using System.Reflection;
using System.Runtime.InteropServices;

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: ComVisible(false)]
[assembly: AssemblyVersion(""$version"")]
[assembly: AssemblyFileVersion(""$version"")]
[assembly: AssemblyInformationalVersion(""$version"")]
[assembly: AssemblyCopyright(""Copyright 2017"")]
[assembly: AssemblyProduct(""$applicationName"")]
[assembly: AssemblyCompany(""Fizz Buzz, Inc."")]
[assembly: AssemblyConfiguration(""$projectConfig"")]"  | out-file $filename -encoding "ASCII"    
}

function script:poke-xml($filePath, $xpath, $value, $namespaces = @{}) {
    [xml] $fileXml = Get-Content $filePath
    
    if($namespaces -ne $null -and $namespaces.Count -gt 0) {
        $ns = New-Object Xml.XmlNamespaceManager $fileXml.NameTable
        $namespaces.GetEnumerator() | %{ $ns.AddNamespace($_.Key,$_.Value) }
        $node = $fileXml.SelectSingleNode($xpath,$ns)
    } else {
        $node = $fileXml.SelectSingleNode($xpath)
    }
    
    Assert ($node -ne $null) "could not find node @ $xpath"
        
    if($node.NodeType -eq "Element") {
        $node.InnerText = $value
    } else {
        $node.Value = $value
    }

    $fileXml.Save($filePath) 
} 