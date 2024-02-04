# Admin and User Role. 
- This is the start example for USer Roles.
- This must be built and run using Vs 2022
- Add roles like the following in Bootstraper.cs
```cs
protected override void InitializeShell(DependencyObject shell)
{
    var identity = WindowsIdentity.GetCurrent();
    var p = new GenericPrincipal(identity, new string[] { "User", "Admin" });
    Thread.CurrentPrincipal = p;
    base.InitializeShell(shell);
}
```
- That can also be done before bootstraper like in App.xaml.cs
```cs
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    ////////////////////////
    var identity = WindowsIdentity.GetCurrent();
    var p = new GenericPrincipal(identity, new string[] { "User", "Admin" });
    Thread.CurrentPrincipal = p;
    ////////////////////////
    var bootstrapper = new BootStrapper();
    bootstrapper.Run();
}
```
- so we got the identity, created a principal object.
- Then assigned some roles to it. Then we set the current principal object.