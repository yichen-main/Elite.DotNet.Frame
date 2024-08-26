namespace Eywa.Domain.ProductionControl.Endpoints.ProductionSchedules.ScheduleTasks;
internal sealed class AddScheduleTaskInput : NodeHeader
{
    [FromBody] public required RawBody Body { get; init; }
    public sealed class RawBody
    {
        public required string MachineId { get; init; }
        public required string PositionNo { get; init; }
        public required string PositionName { get; init; }
        public sealed class Validator : AbstractValidator<AddScheduleTaskInput>
        {
            public Validator(ILocalCulture culture)
            {
                RuleFor(x => x.Body.MachineId).NotEmpty().WithMessage(culture.Link(ProductionControlFlag.MachineIdIsRequired));
                RuleFor(x => x.Body.PositionNo).NotEmpty().WithMessage(culture.Link(ProductionControlFlag.PositionNoIsRequired));
                RuleFor(x => x.Body.PositionName).NotEmpty().WithMessage(culture.Link(ProductionControlFlag.PositionNameIsRequired));
            }
        }
    }
}