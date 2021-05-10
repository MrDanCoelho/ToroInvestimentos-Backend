using FluentMigrator;
using ToroInvestimentos.Backend.Application.Helpers;
using ToroInvestimentos.Backend.Domain.Entities;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;
using ToroInvestimentos.Backend.Domain.Entities.BankClient;
using ToroInvestimentos.Backend.Domain.Entities.Relationship;
using ToroInvestimentos.Backend.Domain.Entities.User;

namespace ToroInvestimentos.Backend.Infra.Migrations
{
    [Migration(1)]
    public class TestMigration : Migration
    {
        public override void Up()
        {
            const string defaultUser = "admin";
            const string defaultPassword = "password";

            #region [ Stock ]

            Create.Table("Stock")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Symbol").AsString(10)
                .WithColumn("Name").AsString(200)
                .WithColumn("CurrentPrice").AsDecimal()
                .WithColumn("Variation").AsDecimal();

            Insert.IntoTable("Stock")
                .Row(new
                    {Symbol = "NOK", Name = "Nokia Corporation", CurrentPrice = 5.02m, Variation = 0.21m})
                .Row(new
                    {Symbol = "TLRY", Name = "Tilray, Inc.", CurrentPrice = 15.30m, Variation = 14.17m})
                .Row(new
                    {Symbol = "LEV", Name = "136213", CurrentPrice = 18.50m, Variation = 17.89m})
                .Row(new
                    {Symbol = "BYND", Name = "Beyond Meat, Inc.", CurrentPrice = 107.73m, Variation = -7.51m})
                .Row(new
                    {Symbol = "BITF.V", Name = "Bitfarms Ltd.", CurrentPrice = 6.80m, Variation = 26.21m})
                .Row(new
                    {Symbol = "ET", Name = "Energy Transfer LP", CurrentPrice = 9.53m, Variation = 7.96m});

            #endregion
            
            #region [ User ]

            Create.Table("User")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString(100)
                .WithColumn("LastName").AsString(100)
                .WithColumn("Email").AsString(100)
                .WithColumn("UserName").AsString(100)
                .WithColumn("Password").AsString(300)
                .WithColumn("PasswordSalt").AsString(100);

            var salt = CryptographyService.CreateSalt(64);
            var password = CryptographyService.HashPassword(defaultPassword + salt);
            
            Insert.IntoTable("User")
                .Row(new
                {
                    FirstName = "Dan", LastName = "Coelho", Email = "dancoelho.contact@gmail.com",
                    UserName = defaultUser, Password = password,
                    PasswordSalt = salt
                });

            #endregion
            
            #region [ Bank Client ]

            Create.Table("BankClient")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(500)
                .WithColumn("Document").AsString(14);
            
            Insert.IntoTable("BankClient")
                .Row(new
                {
                    Name = "Daniel Coelho",
                    Document = "00000000000"
                });

            #endregion

            #region [ Bank Account ]

            Create.Table("BankAccount")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("BankCode").AsString(3)
                .WithColumn("Branch").AsString(4)
                .WithColumn("AccountNumber").AsString(10)
                .WithColumn("BalanceInBrl").AsDecimal();
            
            Insert.IntoTable("BankAccount")
                .Row(new
                {
                    BankCode = "352", Branch = "0001", AccountNumber = "0000000-0", BalanceInBrl = 6000m
                });
            
            Create.Table("BankAccountExchange")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("BankAccountId").AsInt32().ForeignKey("BankAccount", "Id")
                .WithColumn("OriginBankCode").AsString(3)
                .WithColumn("OriginBankBranch").AsString(5)
                .WithColumn("OriginBankAccountNumber").AsString(10)
                .WithColumn("Value").AsDecimal()
                .WithColumn("TransactionDate").AsDate();

            Create.Table("BankAccountStock")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("BankAccountId").AsInt32().ForeignKey("BankAccount", "Id")
                .WithColumn("Amount").AsInt32()
                .WithColumn("Symbol").AsString(10);

            #endregion

            #region [ Relationships ]

            Create.Table("UserBankClient")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("User", "Id")
                .WithColumn("BankClientId").AsInt32().ForeignKey("BankClient", "Id");
            
            Insert.IntoTable("UserBankClient")
                .Row(new
                {
                    UserId = 1,
                    BankClientId = 1
                });
            
            Create.Table("BankClientAccount")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("BankClientId").AsInt32().ForeignKey("BankClient", "Id")
                .WithColumn("BankAccountId").AsInt32().ForeignKey("BankAccount", "Id");
            
            Insert.IntoTable("BankClientAccount")
                .Row(new
                {
                    BankClientId = 1,
                    BankAccountId = 1
                });

            #endregion

            #region [ RefreshToken ]

            Create.Table("RefreshToken")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("User", "Id")
                .WithColumn("Token").AsString(200)
                .WithColumn("Expires").AsDateTime()
                .WithColumn("Created").AsDateTime()
                .WithColumn("CreatedByIp").AsString(19)
                .WithColumn("Revoked").AsDateTime().Nullable()
                .WithColumn("RevokedByIp").AsString(19).Nullable()
                .WithColumn("ReplacedByToken").AsString(200).Nullable()
                .WithColumn("IsActive").AsBoolean();

            #endregion
        }
        
        public override void Down()
        {
            Delete.Table("RefreshToken");
            Delete.Table("Stock");
            
            Delete.Table("User");
            
            Delete.Table("BankAccount");
            Delete.Table("BankAccountExchange");
            Delete.Table("BankAccountStock");
            
            Delete.Table("UserBankClient");
            Delete.Table("BankClientAccount");
        }
    }
}