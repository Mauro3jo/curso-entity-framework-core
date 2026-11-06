$ErrorActionPreference = 'Stop'

function Invoke-Dotnet {
    param([string[]]$Argumentos)
    & dotnet @Argumentos
    if ($LASTEXITCODE -ne 0) { throw "Fallo dotnet: $($Argumentos -join ' ')" }
}

Push-Location $PSScriptRoot
try {
    Invoke-Dotnet -Argumentos @('tool', 'restore')
    foreach ($proyecto in Get-ChildItem -LiteralPath 'ejercicios' -Filter '*.csproj' -Recurse) {
        Invoke-Dotnet -Argumentos @('build', $proyecto.FullName, '--nologo', '--verbosity', 'minimal')
    }
    Invoke-Dotnet -Argumentos @('run', '--project', 'ejercicios/01-code-first/CodeFirst.csproj', '--no-build')
    Invoke-Dotnet -Argumentos @('ef', 'migrations', 'has-pending-model-changes', '--project', 'ejercicios/01-code-first', '--no-build')
    Invoke-Dotnet -Argumentos @('run', '--project', 'ejercicios/02-database-first/DatabaseFirst.csproj', '--no-build')
    Invoke-Dotnet -Argumentos @('run', '--project', 'ejercicios/03-validaciones/Validaciones.csproj', '--no-build')
    Invoke-Dotnet -Argumentos @('run', '--project', 'ejercicios/04-blog/Blog.csproj', '--no-build', '--', '--verificar')
    Invoke-Dotnet -Argumentos @('run', '--project', 'ejercicios/04-blog/Blog.csproj', '--no-build', '--', '--consultas')
    Invoke-Dotnet -Argumentos @('run', '--project', 'ejercicios/05-fluent-api/Academia.csproj', '--no-build', '--', '--verificar')
    Invoke-Dotnet -Argumentos @('run', '--project', 'ejercicios/05-fluent-api/Academia.csproj', '--no-build', '--', '--verificar-sql')
    Invoke-Dotnet -Argumentos @('run', '--project', 'ejercicios/06-mysql/LibrosMySql.csproj', '--no-build')
    Write-Output 'Comprobaciones de consola y SQL Server correctas. HTTP y MySQL real se prueban por separado.'
}
finally { Pop-Location }
