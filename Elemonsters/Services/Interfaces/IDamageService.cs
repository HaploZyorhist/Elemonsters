using Elemonsters.Assets.Creatures;
using Elemonsters.Models.Combat.Requests;
using Elemonsters.Models.Damage.Results;

namespace Elemonsters.Services.Interfaces
{
    public interface IDamageService
    {
        Task<DamageResult> HandleDamage(List<DamageRequest> damageRequests, List<CreatureBase> creatures);
    }
}
