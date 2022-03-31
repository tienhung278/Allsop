using Allsop.Models;
using Allsop.Models.Entities;

namespace Allsop.Contracts
{
    public interface IVoucherRepository
    {
        IQueryable<Voucher> GetVouchers();
        ICollection<Voucher> GetVouchers(QueryParameter parameter);
        Voucher? GetVoucher(int id);        
        void CreateVoucher(Voucher voucher, string userId);
        void UpdateVoucher(Voucher voucher, string userId);
        void DeleteVoucher(Voucher voucher, string userId);
        PageInfo<Voucher> GetVoucherPageInfo(QueryParameter parameter);
    }
}