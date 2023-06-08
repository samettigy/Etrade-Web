using System.ComponentModel.DataAnnotations;

namespace Etrade.Entity.Models.Entities
{
    public enum OrderState
    {
        [Display(Name ="Sipariş Bekleniyor")]
        Waiting,
        [Display(Name ="Sipariş Tamamlandı")]
        Completed
    }
}