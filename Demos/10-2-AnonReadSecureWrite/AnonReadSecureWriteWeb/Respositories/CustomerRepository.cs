using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AnonReadSecureWriteWeb.Respositories {
  public class CustomerRepository : IEntityRepository<Customer> {
    public void Dispose() {
      throw new NotImplementedException();
    }

    public IQueryable<Customer> All { get; private set; }
    public IQueryable<Customer> GetAllIncluding(params Expression<Func<Customer, object>>[] includeProperties) {
      throw new NotImplementedException();
    }

    public IQueryable<Customer> GetMany(Expression<Func<Customer, bool>> @where) {
      throw new NotImplementedException();
    }

    public Customer GetOne(int id) {
      throw new NotImplementedException();
    }

    public Customer GetOne(Expression<Func<Customer, bool>> @where) {
      throw new NotImplementedException();
    }

    public void InsertOrUpdate(Customer entity) {
      throw new NotImplementedException();
    }

    public void Delete(int id) {
      throw new NotImplementedException();
    }
  }
}