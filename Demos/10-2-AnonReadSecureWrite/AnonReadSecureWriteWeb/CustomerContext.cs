using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AnonReadSecureWriteWeb {
  public class CustomerContext :DbContext {
    public CustomerContext() {
    }

    public CustomerContext(SqlConnection sqlConnection)
      : base(sqlConnection, false) {
    }

    public DbSet<Customer> Customers { get; set; }
  }
}