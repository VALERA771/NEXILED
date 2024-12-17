using Exiled.API.Interfaces;
using Respawning.Objectives;

namespace Exiled.API.Features.Objectives
{
    using BaseObjective = Respawning.Objectives.ScpItemPickupObjective;

    public class ScpItemPickupObjective : HumanObjective<PickupObjectiveFootprint>, IWrapper<BaseObjective>
    {
        internal ScpItemPickupObjective(BaseObjective objectiveFootprintBase)
            : base(objectiveFootprintBase)
        {
        }

        public BaseObjective Base { get; }
    }
}