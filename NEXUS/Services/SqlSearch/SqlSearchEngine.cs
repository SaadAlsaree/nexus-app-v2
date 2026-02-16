using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Data.Enums;
using NEXUS.Dtos;

namespace NEXUS.Services.SqlSearch;

public class SqlSearchEngine : ISqlSearchEngine
{
    private readonly AppDbContext _context;

    public SqlSearchEngine(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<SuspectSearchResultDto> Results, int TotalCount)> SearchSuspectsAsync(SuspectSearchCriteria criteria)
    {

        var query = _context.Suspects.AsNoTracking().AsQueryable();

        // genral search
        if (!string.IsNullOrWhiteSpace(criteria.GeneralQuery))
        {
            string q = criteria.GeneralQuery.Trim();
            query = query.Where(s =>
                s.FullName.Contains(q) ||
                s.Kunya!.Contains(q) ||
                (s.MotherName != null && s.MotherName.Contains(q))
            );
        }

        // filters
        if (criteria.Status.HasValue)
        {
            query = query.Where(s => s.CurrentStatus == criteria.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(criteria.Tribe))
        {
            query = query.Where(s => s.Tribe!.Contains(criteria.Tribe));
        }

        if (!string.IsNullOrWhiteSpace(criteria.MotherName))
        {
            query = query.Where(s => s.MotherName!.Contains(criteria.MotherName));
        }

        // filters date range 
        if (criteria.DateOfBirthFrom.HasValue)
        {
            query = query.Where(s => s.DateOfBirth >= criteria.DateOfBirthFrom.Value);
        }
        if (criteria.DateOfBirthTo.HasValue)
        {
            query = query.Where(s => s.DateOfBirth <= criteria.DateOfBirthTo.Value);
        }


        // deep search
        if (!string.IsNullOrWhiteSpace(criteria.PhoneNumber))
        {
            query = query.Where(s => s.Contacts.Any(c => c.Value.Contains(criteria.PhoneNumber)));
        }

        if (!string.IsNullOrWhiteSpace(criteria.AddressCity))
        {
            query = query.Where(s => s.Addresses.Any(a => a.City.Contains(criteria.AddressCity) || a.District.Contains(criteria.AddressCity)));
        }

        if (!string.IsNullOrWhiteSpace(criteria.OrgUnitName))
        {
            query = query.Where(s => s.OrganizationalAssignments.Any(a => a.OrgUnit.UnitName.Contains(criteria.OrgUnitName)));
        }

        if (criteria.InvolvedInOperationType.HasValue)
        {
            query = query.Where(s => s.Operations.Any(op => op.OperationType == criteria.InvolvedInOperationType.Value));
        }

        // Execution & Projection
        int totalCount = await query.CountAsync();

        query = query.OrderByDescending(s => s.CreatedAt);


        var results = await query
            .Skip((criteria.PageNumber - 1) * criteria.PageSize)
            .Take(criteria.PageSize)
            .Select(s => new SuspectSearchResultDto
            {
                Id = s.Id,
                FullName = s.FullName,
                Kunya = s.Kunya!,
                Status = s.CurrentStatus.ToString(),

                // جلب آخر عنوان مسجل (مثال على Sub-query بسيطة)
                CurrentLocation = s.Addresses
                    .OrderByDescending(a => a.CreatedAt)
                    .Select(a => a.City + " - " + a.District)
                    .FirstOrDefault() ?? "غير محدد",

                // جلب آخر منصب
                LastRole = s.OrganizationalAssignments
                    .OrderByDescending(a => a.StartDate)
                    .Select(a => a.RoleTitle.ToString())
                    .FirstOrDefault() ?? "عنصر",

                // عدد القضايا
                CaseCount = s.CaseInvolvements.Count()
            })
            .ToListAsync();

        return (results, totalCount);
    }

    // Cases Search
    public async Task<List<Case>> SearchCasesAsync(string queryText, CaseStatus? status)
    {
        var query = _context.Cases.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryText))
        {
            query = query.Where(c => c.CaseFileNumber.Contains(queryText) || c.Title.Contains(queryText));
        }

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        return await query.Take(50).ToListAsync();
    }
}