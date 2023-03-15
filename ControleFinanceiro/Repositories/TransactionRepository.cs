using ControleFinanceiro.Models;
using LiteDB;


namespace ControleFinanceiro.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly LiteDatabase _database;
        private readonly string colletionName = "transactions";
        public TransactionRepository(LiteDatabase database) 
        {
            _database = database;
        }

        public List<Transaction> GetAll()
        {
            return _database.GetCollection<Transaction>(colletionName).Query().OrderByDescending(x => x.Date).ToList();
        }
        public void Add(Transaction transaction)
        {
            var collection = _database.GetCollection<Transaction>(colletionName);
            collection.Insert(transaction);
            collection.EnsureIndex(x => x.Date);
        }
        public void Update(Transaction transaction)
        {
            var collection = _database.GetCollection<Transaction>(colletionName);
            collection.Update(transaction);
        }
        public void Delete(Transaction transaction)
        {
            var collection = _database.GetCollection<Transaction>(colletionName);
            collection.Delete(transaction.Id);
        }
    }
}
