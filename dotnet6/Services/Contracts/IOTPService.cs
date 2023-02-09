namespace DigitalBanking.Services.Contracts
{
    public interface IOTPService
    {
        public void GenerateOTP();

        public bool ValidateOTP();

    }


    public interface IAccountService
    {
        public void getAccounts();
    }


    public class AccountService : IAccountService
    {
        public void getAccounts()
        {
            
        }
    }
}
