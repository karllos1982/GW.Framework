using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;
using GW.Helpers;

namespace GW.Membership.Domain
{
    public class UserDomain : IUserDomain
    {
        public UserDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet =repositorySet;  
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public UserModel Get(UserParam param)
        {
            UserModel ret = null;

            ret = RepositorySet.User.Read(param); 
            
            return ret;
        }

        public List<UserList> List(UserParam param)
        {
            List<UserList> ret = null;

            ret = RepositorySet.User.List(param);           

            return ret;
        }

        public List<UserSearchResult> Search(UserParam param)
        {
            List<UserSearchResult> ret = null;

            ret = RepositorySet.User.Search(param);

            return ret;
        }

        public OperationStatus Set(UserModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            ret = EntryValidation(model);

            if (ret.Status)
            {

                UserModel old 
                    = RepositorySet.User.Read(new UserParam() { pUserID = model.UserID });

                if (old == null)
                {
                    ret = InsertValidation(model);

                    if (ret.Status)
                    {
                        model.CreateDate = DateTime.Now;
                        ret = RepositorySet.User.Create(model);
                    }
                }
                else
                {
                    model.CreateDate = old.CreateDate;
                    operation = OPERATIONLOGENUM.UPDATE;

                    ret = UpdateValidation(model);

                    if (ret.Status)
                    {
                        ret = RepositorySet.User.Update(model);
                    }

                }

                if (ret.Status && userid != null)
                {
                    RepositorySet.User.Context
                        .RegisterDataLog(userid.ToString(), operation, "SYSUSER",
                        model.UserID.ToString(), old, model);

                    ret.Returns = model;
                }

            }     

            return ret;
        }

        public void FillChields(ref UserModel obj)
        {
            
        }

        public OperationStatus Delete(UserModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);

            UserModel old 
                = RepositorySet.User.Read(new UserParam() { pUserID = model.UserID });

            if (old != null)
            {
                ret = DeleteValidation(model);

                if (ret.Status)
                {
                    ret = RepositorySet.User.Delete(model);
                }
            }
            else
            {
                ret.Status = false;
                ret.Error = new System.Exception(GW.Localization.GetItem("Record-NotFound").Text);

            }           

            return ret;
        }


        public OperationStatus EntryValidation(UserModel obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>());

            if (!ret.Status)
            {
                ret.Error = new Exception(GW.Localization.GetItem("Validation-Error").Text);
            }

            Context.ExecutionStatus = ret; 

            return ret;
        }           
             
        public OperationStatus InsertValidation(UserModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            UserParam param = new UserParam()
            {
                pUserName = obj.UserName
            };

            List<UserList> list
                = RepositorySet.User.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ret.Status = false;
                    ret.Error = new Exception(GW.Localization.GetItem("Validation-Unique-Value").Text);
                }
            }

            Context.ExecutionStatus = ret;

            return ret;
        }
            
        public OperationStatus UpdateValidation(UserModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            UserParam param = new UserParam() { pUserName = obj.UserName };
            List<UserList> list
                = RepositorySet.User.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    if (list[0].UserID != obj.UserID)
                    {
                        ret.Status = false;
                        ret.Error = new Exception(GW.Localization.GetItem("Validation-Unique-Value").Text);
                    }
                }
            }

            Context.ExecutionStatus = ret;

            return ret;

        }

        public OperationStatus DeleteValidation(UserModel obj)
        {
            return new OperationStatus(true); 
        }


    }
}
