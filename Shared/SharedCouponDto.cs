namespace Shared.CouponDTOs;


public class CouponDTO{
    public int coupon_id {get; set;}
    public string coupon_name {get; set;}
    public int coupon_count {get; set;}
    public int coupon_discount {get; set;}
    public DateTime coupon_expiredate {get; set;}

    public CouponDTO()
    {
        this.coupon_id = -1;
        this.coupon_name = "";
        this.coupon_count = -1;
        this.coupon_discount = -1;
        this.coupon_expiredate = DateTime.UtcNow;
    }

    public CouponDTO(int id, string name, int count, int discount, DateTime expiredate)
    {
        this.coupon_id = id;
        this.coupon_name = name;
        this.coupon_count = count;
        this.coupon_discount = discount;
        this.coupon_expiredate = expiredate;
    }
    
}
