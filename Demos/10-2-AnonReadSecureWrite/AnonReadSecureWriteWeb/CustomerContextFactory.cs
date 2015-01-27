using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AnonReadSecureWriteWeb {
  public class CustomerContextFactory :IDbContextFactory<CustomerContext>
  {
    public CustomerContext Create() {
      // get connection string from autohosted db
      string connectionString = WebConfigurationManager.AppSettings["SqlAzureConnectionString"];

      // create DB connection
      SqlConnection sqlConnection = new SqlConnection(connectionString);

      return new CustomerContext(sqlConnection);
    }
  }
}