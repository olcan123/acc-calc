using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate
{
    public class PurchaseInvoiceExpenseManager : IPurchaseInvoiceExpenseService
    {
        private readonly IPurchaseInvoiceExpenseDal _purchaseInvoiceExpenseDal;


        public PurchaseInvoiceExpenseManager(IPurchaseInvoiceExpenseDal purchaseInvoiceExpenseDal)
        {
            _purchaseInvoiceExpenseDal = purchaseInvoiceExpenseDal;
        }

        public IDataResult<List<PurchaseInvoiceExpense>> GetList()
        {
            var result = _purchaseInvoiceExpenseDal.GetAll();
            return new SuccessDataResult<List<PurchaseInvoiceExpense>>(result);
        }

        public IDataResult<List<PurchaseInvoiceExpense>> GetListByInvoiceId(int invoiceId)
        {
            var result = _purchaseInvoiceExpenseDal.GetAll(x => x.PurchaseInvoiceId == invoiceId);
            return new SuccessDataResult<List<PurchaseInvoiceExpense>>(result);
        }

        public IDataResult<PurchaseInvoiceExpense> GetById(int id)
        {
            var result = _purchaseInvoiceExpenseDal.Get(x => x.Id == id);
            return new SuccessDataResult<PurchaseInvoiceExpense>(result);
        }

        [ValidationAspect(typeof(PurchaseInvoiceExpenseValidator))]
        public IResult Add(PurchaseInvoiceExpense purchaseInvoiceExpense)
        {

            _purchaseInvoiceExpenseDal.Add(purchaseInvoiceExpense);
            return new SuccessResult("Fatura masrafı eklendi");
        }

        [ValidationAspect(typeof(PurchaseInvoiceExpenseValidator))]
        public IResult Update(PurchaseInvoiceExpense purchaseInvoiceExpense)
        {
            _purchaseInvoiceExpenseDal.Update(purchaseInvoiceExpense);
            return new SuccessResult("Fatura masrafı güncellendi");
        }

        public IResult Delete(PurchaseInvoiceExpense purchaseInvoiceExpense)
        {
            _purchaseInvoiceExpenseDal.Delete(purchaseInvoiceExpense);
            return new SuccessResult("Fatura masrafı silindi");
        }

        //SECTION - Bulk        [ValidationAspect(typeof(PurchaseInvoiceExpenseValidator))]
        public IResult BulkAdd(List<PurchaseInvoiceExpense> purchaseInvoiceExpenses)
        {
            // Use AddRange instead of BulkAdd to avoid enum casting issues with EFCore.BulkExtensions
            _purchaseInvoiceExpenseDal.AddRange(purchaseInvoiceExpenses);
            return new SuccessResult("Fatura masrafları eklendi");
        }

        [ValidationAspect(typeof(PurchaseInvoiceExpenseValidator))]
        public IResult BulkUpdate(List<PurchaseInvoiceExpense> purchaseInvoiceExpenses)
        {
            var purchaseInvoiceId = purchaseInvoiceExpenses.FirstOrDefault()?.PurchaseInvoiceId;
            if (purchaseInvoiceId == null)
                return new ErrorResult("Fatura ID'si bulunamadı");

            _purchaseInvoiceExpenseDal.MergeSync(purchaseInvoiceExpenses, (source, target) =>
                source.Id == target.Id,
                target => target.PurchaseInvoiceId == purchaseInvoiceId.Value);
            return new SuccessResult("Fatura masrafları güncellendi");
        }

        public IResult BulkDeleteByPurchaseInvoiceId(int purchaseInvoiceId)
        {
            var purchaseInvoiceExpenses = _purchaseInvoiceExpenseDal.GetAll(x => x.PurchaseInvoiceId == purchaseInvoiceId);
            if (purchaseInvoiceExpenses != null && purchaseInvoiceExpenses.Count > 0)
            {
                _purchaseInvoiceExpenseDal.BulkDelete(purchaseInvoiceExpenses);
                return new SuccessResult("Fatura masrafları silindi");
            }
            else
            {
                return new ErrorResult("Fatura masrafı bulunamadı");
            }
        }
    }
}
