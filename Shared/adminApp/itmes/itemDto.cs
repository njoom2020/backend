
namespace Shared.ItemAdminDTOs;
public class ItemAdminDTO
{
    public ItemAdminDTO(int itmes_id, string itmes_name, string itmes_nameAr, int itmes_count, bool itmes_active, int items_categoryId,string items_image,DateTime items_created,string categoryName,string categoryNameAr)
    {
          this.Items_id     =   itmes_id    ;
          this.Items_name   =   itmes_name  ;
          this.Items_nameAr =   itmes_nameAr;
          this.Items_count  =   itmes_count ;
          this.Items_active =   itmes_active;
          this.Items_categoryId = items_categoryId;
          this.Items_image = items_image;
          this.Items_created = items_created;
          this.CategoryName = categoryName;
          this.CategoryNameAr = categoryNameAr;
    }
    public int Items_id {get; set;}
    public string Items_name {get; set;} = "";
    public string Items_nameAr {get;set;} = "";
    public int Items_count {get; set;}
    public bool Items_active {get; set;}
    public int Items_categoryId {get; set;}
    public string Items_image {get;set;}
    public DateTime Items_created {get;set;}
    public string CategoryName {get;set;}
    public string CategoryNameAr {get;set;}
}


public class ItemInfoAdminDTO
{
    public ItemInfoAdminDTO( string itmes_desc, string itmes_descAr, decimal itmes_price, int itmes_discount)
    {
          this.Items_desc   =   itmes_desc  ;
          this.Items_descAr =   itmes_descAr;
          this.Items_price  =   itmes_price ;
          this.Items_discount =   itmes_discount;
    }
    public string Items_desc {get; set;} = "";
    public string Items_descAr {get;set;} = "";
    public decimal Items_price {get;set;}
    public int Items_discount {get; set;}
}
