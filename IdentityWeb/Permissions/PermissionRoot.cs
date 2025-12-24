namespace IdentityWeb.Permissions
{
    public static class PermissionRoot
    {

        public static class Stock
        {
            public const string Read = "Permissions.Stock.Read";
            public const string Create = "Permissions.Stock.Create";
            public const string Delete = "Permissions.Stock.Delete";
            public const string Update = "Permissions.Stock.Update";
        }
        public static class Order
        {
            public const string Read = "Permissions.Order.Read";
            public const string Create = "Permissions.Order.Create";
            public const string Delete = "Permissions.Order.Delete";
            public const string Update = "Permissions.Order.Update";
        }

    }


}
