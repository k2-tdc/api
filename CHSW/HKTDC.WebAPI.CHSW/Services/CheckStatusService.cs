using HKTDC.WebAPI.CHSW.DBContext;
using HKTDC.WebAPI.CHSW.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Services
{
    public class CheckStatusService : CommonService
    {
        private EntityContext Db = new EntityContext();

        /// <summary>
        /// Used to return all three level of services 
        /// Table  : ServiceType
        /// get the data from Table and bind the value to ServiceType list
        /// </summary>
        /// <returns>Return list : ServiceLevel1</returns>
        public List<ServiceLevel1> GetAllServiceTypes()
        {
            List<ServiceLevel1> ServiceList = new List<ServiceLevel1>();
            List<ServiceType> ServiceTypes = new List<ServiceType>();

            try
            {
                ServiceTypes = Db.ServiceTypes.ToList();
                foreach (var FirstlevelOption in ServiceTypes.Where(P => P.ServiceTypeLevel == "1").OrderBy(P => P.DisplayOrder))  // to filter the 1st level services
                {
                    if (FirstlevelOption != null)
                    {
                        ServiceLevel1 Level1 = new ServiceLevel1();
                        Level1.Name = FirstlevelOption.ServiceTypeName;
                        Level1.GUID = FirstlevelOption.ServiceTypeGUID;
                        List<ServiceLevel2> Level2List = new List<ServiceLevel2>();
                        foreach (var SecondLevelOption in ServiceTypes.Where(P => P.ParentServiceTypeGUID == FirstlevelOption.ServiceTypeGUID.ToString()).OrderBy(P => P.DisplayOrder)) // to filter the 2nd Level Services 
                        {
                            if (SecondLevelOption != null)
                            {
                                ServiceLevel2 Level2 = new ServiceLevel2();
                                Level2.Name = SecondLevelOption.ServiceTypeName;
                                Level2.GUID = SecondLevelOption.ServiceTypeGUID;
                                List<ServiceLevel3> Level3List = new List<ServiceLevel3>();
                                foreach (var ThirdLevelOption in ServiceTypes.Where(P => P.ParentServiceTypeGUID == SecondLevelOption.ServiceTypeGUID.ToString()).OrderBy(P => P.DisplayOrder)) // to filter the 3rd Level Services
                                {
                                    if (ThirdLevelOption != null)
                                    {
                                        ServiceLevel3 Level3 = new ServiceLevel3();
                                        Level3.Name = ThirdLevelOption.ServiceTypeName;
                                        Level3.GUID = ThirdLevelOption.ServiceTypeGUID;
                                        Level3.Approver = ThirdLevelOption.ApproverRuleCode;
                                        Level3.ActionTaker = ThirdLevelOption.ActionTakerRuleCode;
                                        Level3.Enabled = ThirdLevelOption.Enabled;
                                        Level3.ControlFlag = ThirdLevelOption.ControlFlag;
                                        Level3.Placeholder = ThirdLevelOption.Placeholder;
                                        Level3List.Add(Level3);
                                    }
                                }
                                if (Level3List.Count > 0)
                                    Level2.Level3 = Level3List;
                                else
                                    Level2.Level3 = null;
                                Level2List.Add(Level2);
                            }
                        }
                        if (Level2List.Count > 0)
                            Level1.Level2 = Level2List;
                        else
                            Level1.Level2 = null;
                        ServiceList.Add(Level1);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceList = null;
                throw ex;

            }
            return ServiceList;
        }

        public List<VWEmployeeDTO> GetForwardEmployee(string applicant)
        {
            try
            {
                List<Applicant> employees = new List<Applicant>();
                SqlParameter[] sqlp = { new SqlParameter("UserID", applicant) };
                employees = Db.Database.SqlQuery<Applicant>("exec [pProcessActionTakerGet] @UserID", sqlp).ToList(); // Refer the pProcessWorkerGet SP From  Workflow DataBase

                List<VWEmployeeDTO> employeeDTO = new List<VWEmployeeDTO>();
                employeeDTO = employees.Select(p => new VWEmployeeDTO
                {
                    UserId = p.WorkerId,
                    UserFullName = p.WorkerFullName
                }).OrderBy(p => p.UserFullName).ToList();

                return employeeDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}