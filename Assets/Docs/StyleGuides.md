# Style Guides

1. Naming:
    - Functions PascalCase
    - Classes PascalCase
    - Namespaces PascalCase
    - Attributes camelCase
    - Functions that return bool `IsSth` `HasSth`
    - Functions that are event listeners `OnEvent`
    - Private attributes `_sth`
    - Bool variables `isSth` `hasSth`
    - Interfaces `ISth`
2. Code organization:
    - always explicits sa that method/attribute is private
    - if you require same other component on the same game object use `[RequireComponent(typeof(Sth))]`
    - hide all public attributes that should not to be eddied from inspector `[HideInInspector]`
3. Optimizations:
    - Avoid using:
        - `Random` - it's expensive so don't use it in Update (every frame)
        - `GameObject.Find...` - it's iterating every loaded object, don't use it. If you need to get reference use public field's assigned in inspector or use EventManager singleton and others Mangers.
        - `Vector.magnitude` - if you don't need use `sqrMagnitude` (SquareRoot calculations are expensive)
