Installing the tools
dotnet ef can be installed as either a global or local tool. Most developers prefer installing dotnet ef as a global tool using the following command:

.NET CLI

Copy
dotnet tool install --global dotnet-ef
To use it as a local tool, restore the dependencies of a project that declares it as a tooling dependency using a tool manifest file.

Update the tool using the following command:

.NET CLI

Copy
dotnet tool update --global dotnet-ef
Before you can use the tools on a specific project, you'll need to add the Microsoft.EntityFrameworkCore.Design package to it.

.NET CLI

Copy
dotnet add package Microsoft.EntityFrameworkCore.Design

Verify installation
Run the following commands to verify that EF Core CLI tools are correctly installed:

.NET CLI

Copy
dotnet ef

Update the tools
Use dotnet tool update --global dotnet-ef 

Revert DB to c# code

dotnet ef dbcontext scaffold "Server=.;Database=AdventureWorks2019;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models


dotnet ef dbcontext scaffold "Server=.;Database=AdventureWorks2019;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -t Blog -t Post --context-dir Context -c AdventureWorksContext --context-namespace New.Namespace

dotnet ef dbcontext script

dotnet ef migrations has-pending-model-changes
