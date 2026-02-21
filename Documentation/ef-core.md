# Add-Migration
`dotnet ef migrations add [NAME] --project Fitness.DataAccess --startup-project Fitness`

# Remove-Migration
`dotnet ef migrations remove --project Fitness.DataAccess --startup-project Fitness`

# Update-Database
`dotnet ef database update [NAME (Muss aber nicht sein)] --project Fitness.DataAccess --startup-project Fitness`

# Script-Migration
`Script-Migration -From [FROM] -To [TO]`