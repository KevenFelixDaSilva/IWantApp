using Dapper;
using IWantApp.Endpoints;
using IWantApp.Endpoints.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Data;

public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IEnumerable<EmployeeResponse> Execute(int page, int rows)
    {
        var db = new SqlConnection(configuration["ConnectionString:IWantDb"]);
        var query = 
            @"SELECT Email, ClaimValue
            FROM AspNetUsers u INNER 
            JOIN AspNetUserClaims c
            ON u.id = c.userId and claimtype = 'Name'
            ORDER BY NAME
            OFFSETS (@page -1)* @rows ROWS FETCH NEXT @rows ROWS ONLY";
        return db.Query<EmployeeResponse>(
            query,
            new { page, rows }
        );  
    }
}
