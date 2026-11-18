using EcommerceApiDataLayer;
using Shared.CouponDTOs;

namespace EcommerceApiBusinessLayer;

public class Coupon
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;

    public CouponDTO CDTO
    {
        get
        {
            return new CouponDTO(this.couponId, this.couponName, this.couponCount, this.couponDiscount, this.couponExpiredate);
        }
    }

    public int couponId { get; set; }
    public string couponName { get; set; }
    public int couponCount { get; set; }
    public int couponDiscount { get; set; }
    public DateTime couponExpiredate { get; set; }

    public Coupon(CouponDTO couponDto, enMode cMode = enMode.AddNew)
    {
        this.couponId = couponDto.coupon_id;
        this.couponName = couponDto.coupon_name;
        this.couponCount = couponDto.coupon_count;
        this.couponDiscount = couponDto.coupon_discount;
        this.couponExpiredate = couponDto.coupon_expiredate;

        Mode = cMode;
    }

    public static CouponDTO checkCoupon(string couponName)
    {
        return CouponData.checkCoupon(couponName);
    }


}