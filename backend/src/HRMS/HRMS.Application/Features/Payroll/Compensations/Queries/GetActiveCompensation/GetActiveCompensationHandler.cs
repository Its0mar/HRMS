using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Features.Payroll.Compensations.Dtos;

namespace HRMS.Application.Features.Payroll.Compensations.Queries.GetActiveCompensation
{
    public record GetActiveCompensationHandler(IPayrollRepository Repository, ICurrentUser CurrentUser)
        : IQueryHandler<GetActiveCompensationQuery, GetActiveCompensationResponse>
    {
        public async Task<ErrorOr<GetActiveCompensationResponse>> HandleAsync(GetActiveCompensationQuery query, CancellationToken cancellationToken)
        {
            var result =  await Repository.GetActiveCompensationAsync(query.EmployeeId, CurrentUser.OrganizationId, cancellationToken);
           
            if (result is null)
            {
                return Error.NotFound(description: "Active compensation not found.");
            }

            var totalCompensation = result.BasicSalary + result.Allowances.Sum(a => a.Amount) - result.Deductions.Sum(d => d.Amount);
            var allowances = result.Allowances.Select(a => new CompensationAllowanceDto(a.Title, a.Amount)).ToList();
            var deductions = result.Deductions.Select(a => new CompensationDeductionDto(a.Title, a.Amount)).ToList();


            var response = new GetActiveCompensationResponse(
                result.Id ?? -1,
                result.EmployeeId,
                result.BasicSalary,
                result.Currency,
                result.PayFrequency.ToString(),
                result.EffectiveFrom,
                result.CreatedAt,
                allowances,
                deductions,
                totalCompensation
                );

            return response;
        
        }

    }
}
