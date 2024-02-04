# Admin and User Role. 
- In the shell project make a folder called Core, and then adda class called RoleBasedModuleInitializer.
- Copy this code, https://github.com/PrismLibrary/Prism/blob/master/src/Wpf/Prism.Wpf/Modularity/ModuleInitializer.cs
- Modify public void Initialize(IModuleInfo moduleInfo)

```cs
public void Initialize(IModuleInfo moduleInfo)
{
    if (moduleInfo == null)
        throw new ArgumentNullException(nameof(moduleInfo));
    IModule moduleInstance = null!;
    try
    {
        if (ModuleIsInUserRole(moduleInfo))
        {
            moduleInstance = this.CreateModule(moduleInfo);
            if (moduleInstance != null)
            {
                moduleInstance.RegisterTypes(_containerExtension);
                moduleInstance.OnInitialized(_containerExtension);
            }
        }
    }
    catch (Exception ex)
    {
        this.HandleModuleInitializationError(
            moduleInfo, moduleInstance?.GetType().Assembly.FullName!, ex);
    }
}

private bool ModuleIsInUserRole(IModuleInfo moduleInfo)
{
    bool isInRole = false;
    var roles = GetModuleRoles(moduleInfo);
    foreach (var role in roles)
    {
        if (WindowsPrincipal.Current!.IsInRole(role))
        {
            isInRole = true;
            break;
        }
    }
    return isInRole;
}

private IEnumerable<string> GetModuleRoles(IModuleInfo moduleInfo)
{
    var type = Type.GetType(moduleInfo.ModuleType);
    if (type == null)
        return null!;
    foreach (var attr in GetCustomAttribute<RolesAttribute>(type!))
        return attr.Roles.AsEnumerable();
    
    return null!;
}

private IEnumerable<T> GetCustomAttribute<T>(Type type)
{
    return type.GetCustomAttributes(typeof(T), true).OfType<T>();
}
```

- Register this new class in the bootstraper.

```cs
protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    containerRegistry.RegisterSingleton<IModuleInitializer, RoleBasedModuleInitializer>();
}
```

- Now run the app with various combinations of user roles. Modules will be loaded selectively.
```cs
protected override void InitializeShell(DependencyObject shell)
{
    var identity = WindowsIdentity.GetCurrent();
    // var p = new GenericPrincipal(identity, new string[] { "User", "Admin" });
    // var p = new GenericPrincipal(identity, new string[] { "User" });
    var p = new GenericPrincipal(identity, new string[] { "Admin" });
    Thread.CurrentPrincipal = p;
    base.InitializeShell(shell);
}
```

- Feel free to take this approach, apply it to your application and modify it to match your requirements, so if you need claims-based authorization, well, change this to use your claims instead of using a hard coded role name instead.