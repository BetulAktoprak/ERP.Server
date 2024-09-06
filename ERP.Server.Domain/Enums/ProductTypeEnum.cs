using Ardalis.SmartEnum;

namespace ERP.Server.Domain.Enums;
public sealed class ProductTypeEnum : SmartEnum<ProductTypeEnum>
{
    public static ProductTypeEnum YariMamul = new("Yari Mamül", 1);
    public static ProductTypeEnum Mamul = new("Mamül", 2);
    public ProductTypeEnum(string name, int value) : base(name, value)
    {
    }
}
