# Admin and User Role. 
- Add a new projecjt.
- Create a new class inside of that project.
- Add a class RolesAttribute.
- Now all other projects should have reference to this project.
- Add attributes as follows in Admin and User module.
```cs
[Roles("Admin")]
public class AdminModule : IModule{ }
```

```cs
[Roles("User")]
public class UserModule : IModule{ }
```
- 