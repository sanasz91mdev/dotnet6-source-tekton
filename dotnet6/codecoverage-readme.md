
Command below  outputs the xml based cverage report to an output directory:

    dotnet test --collect:"XPlat Code Coverage"

Output Attachments:

    /home/sana/dotnet6/Digital Banking tests/TestResults/25033640-7f1d-4097-9850-d650578839e6/coverage.cobertura.xml

Use above directory to generate readable HTML report

    reportgenerator -reports:"/home/sana/dotnet6/Digital Banking tests/TestResults/25033640-7f1d-4097-9850-d650578839e6/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

This outputs:

    sana@sana1:~/dotnet6/AppNet6$ reportgenerator -reports:"/home/sana/dotnet6/Digital Banking tests/TestResults/25033640-7f1d-4097-9850-d650578839e6/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
    2022-01-28T10:06:46: Arguments
    2022-01-28T10:06:46:  -reports:/home/sana/dotnet6/Digital Banking tests/TestResults/25033640-7f1d-4097-9850-d650578839e6/coverage.cobertura.xml
    2022-01-28T10:06:46:  -targetdir:coveragereport
    2022-01-28T10:06:46:  -reporttypes:Html
    2022-01-28T10:06:47: Writing report file 'coveragereport/index.html'
    2022-01-28T10:06:47: Report generation took 0.8 seconds

Run 

    coveragereport/index.html

in browser to view coverage report.