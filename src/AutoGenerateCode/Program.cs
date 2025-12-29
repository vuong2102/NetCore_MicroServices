// --- C?u hình ---
const string ProjectName = "Catalog.API";
string rootDirectory = FindSolutionRoot(Directory.GetCurrentDirectory());

// --- Lu?ng chính ---
// 1. Ch?n d? án m?c tiêu
string? selectedProjectPath = SelectProject(rootDirectory);
if (selectedProjectPath == null) return;
string projectName = Path.GetFileName(selectedProjectPath);

// 2. L?y danh sách template
var templates = GetEmbeddedTemplates();
if (templates == null) return;

// 3. Project c?n Generate code
var selectedTemplateResource = ShowAndSelectTemplate(templates);
var entityName = GetEntityInput();
var actions = GetActionDefinitions(entityName);

GenerateFeatures(entityName, actions, selectedTemplateResource);

Console.WriteLine($"\n? Done! All CRUD for {entityName.Pluralize()} is ready.");


// ================================== HÀM CHI TI?T ==========================================

// 1. Hàm m?i: Quét và ch?n Project cùng c?p
string? SelectProject(string rootDir)
{
    var projectFiles = Directory.EnumerateFiles(rootDir, "*.csproj", SearchOption.AllDirectories)
        .Where(p => !p.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                    !p.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") &&
                    !Path.GetFileName(p).Equals("AutoGenerateCode.csproj", StringComparison.OrdinalIgnoreCase))
        .Select(p => new { ProjectFile = p, ProjectDir = Path.GetDirectoryName(p)! })
        .ToList();

    if (!projectFiles.Any())
    {
        Console.WriteLine("No project files (.csproj) found.");
        return null;
    }

    Console.WriteLine("======= SELECT TARGET PROJECT ========");
    for (int i = 0; i < projectFiles.Count; i++)
    {
        var projectName = Path.GetFileNameWithoutExtension(projectFiles[i].ProjectFile);
        Console.WriteLine($"{i + 1}. {projectName}");
    }

    while (true)
    {
        Console.Write("\nEnter project number: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= projectFiles.Count)
        {
            return projectFiles[choice - 1].ProjectDir;
        }
        Console.WriteLine("Invalid selection.");
    }
}
// 2. L?y danh sách file template
List<string>? GetEmbeddedTemplates()
{
    var assembly = Assembly.GetExecutingAssembly();
    var templates = assembly.GetManifestResourceNames()
                            .Where(n => n.Contains(".Templates.", StringComparison.OrdinalIgnoreCase) &&
                                        n.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                            .ToList();
    if (!templates.Any())
    {
        Console.WriteLine("? No embedded templates found.");
        return null;
    }
    return templates;
}

// 2. Giao di?n ch?n Template
string ShowAndSelectTemplate(List<string> templates)
{
    Console.WriteLine("======= SELECT TEMPLATE ========");
    for (int i = 0; i < templates.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {GetTemplateDisplayName(templates[i])}");
    }

    while (true)
    {
        Console.Write("\nEnter template number: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= templates.Count)
        {
            return templates[choice - 1];
        }
        Console.WriteLine("?? Invalid selection, please try again.");
    }
}

// 3. L?y tên Entity t? ngu?i dùng
string GetTemplateDisplayName(string resourceName)
{
    const string marker = ".Templates.";
    var index = resourceName.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
    return index >= 0 ? resourceName[(index + marker.Length)..] : resourceName;
}
string GetEntityInput()
{
    Console.Write("Enter entity name (e.g., Product, Category): ");
    string input = Console.ReadLine() ?? "Product";
    return input.Dehumanize().Pascalize();
}

// 4. Ð?nh nghia các Action CRUD
dynamic[] GetActionDefinitions(string entityName)
{
    var singular = entityName.Pascalize();
    var plural = entityName.Pluralize().Pascalize();

    return new dynamic[]
    {
        new { Action = "Create",  Method = "MapPost",   Suffix = singular, HasId = false },
        new { Action = "GetList", Method = "MapGet",    Suffix = plural,   HasId = false },
        new { Action = "GetById", Method = "MapGet",    Suffix = singular, HasId = true },
        new { Action = "Update",  Method = "MapPut",    Suffix = singular, HasId = true },
        new { Action = "Delete",  Method = "MapDelete", Suffix = singular, HasId = true }
    };
}

// 5. Logic render và ghi file
void GenerateFeatures(string entityName, dynamic[] actions, string templateResourceName)
{
    var singularEntity = entityName.Pascalize();
    var pluralEntity = entityName.Pluralize().Pascalize();
    var routeBase = pluralEntity.ToLower();

    var templateContent = ReadEmbeddedTemplate(templateResourceName);
    var template = Template.Parse(templateContent);

    // Gi? d?nh Console app n?m cùng c?p folder v?i d? án API
    var baseDir = Path.Combine(selectedProjectPath!, pluralEntity);

    foreach (var item in actions)
    {
        string featureName = $"{item.Action}{item.Suffix}";
        string featurePath = Path.Combine(baseDir, featureName);
        Directory.CreateDirectory(featurePath);

        string routeName = item.HasId ? $"{routeBase}/{{id}}" : routeBase;

        var result = template.Render(new
        {
            project_name = projectName,
            group_name = pluralEntity,
            feature_name = featureName,
            entity_name = singularEntity,
            action_type = item.Action,
            http_method = item.Method,
            route_name = routeName,
            fields = "/* Thêm field ? dây */"
        });

        string fileName = $"{featureName}.cs";
        File.WriteAllText(Path.Combine(featurePath, fileName), result);
        Console.WriteLine($"[Created] -> {featureName}/{fileName}");
    }
}
string ReadEmbeddedTemplate(string resourceName)
{
    var assembly = Assembly.GetExecutingAssembly();
    using var stream = assembly.GetManifestResourceStream(resourceName);
    if (stream == null)
    {
        throw new FileNotFoundException($"Embedded template not found: {resourceName}");
    }
    using var reader = new StreamReader(stream);
    return reader.ReadToEnd();
}

string FindSolutionRoot(string startDir)
{
    var dir = new DirectoryInfo(startDir);
    while (dir != null)
    {
        if (dir.GetFiles("*.sln").Any() || dir.GetFiles("*.slnx").Any())
        {
            return dir.FullName;
        }
        dir = dir.Parent;
    }
    return startDir;
}
