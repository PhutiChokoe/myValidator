using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using MyValidatorService.Database;
using MyValidatorService.Enums;
using MyValidatorService.Interfaces;
using NLog;

namespace MyValidatorService.Models
{
    public class IdValidator 
    {
        private const string DateFormat = "yyMMdd";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly List<ErrorLists> _errorList = new List<ErrorLists>();
        private readonly IdBreakDown _id = new IdBreakDown();
        private readonly UserDetail _userDetails = new UserDetail();
        private IDbHandler _iDbHandler { get; set; }




        public bool IsIdLengthValid(string id)
        {
            try
            {
                if (id.Length != 13)
                {
                    _errorList.Add(new ErrorLists {error = ErrorList.InCorrectIdLength.ToString()});
                    return false;
                }

                else
                {
                    _userDetails.Id = long.Parse(id);
                    BreakDownId(id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
        }

        public void BreakDownId(string id)
        {
            _id.Date = id.Substring(0, 6);
            _id.Gender = id.Substring(6, 4);
            _id.Citizenship = id.Substring(10, 1);
            _id.SecondLastDigit = id.Substring(11, 1);
            _id.ControlDigit = id.Substring(12, 1);
        }

        public JsonResponse ValidateId(string id)
        {
            try
            {
                _iDbHandler = new DbHandler();
                if (IsIdLengthValid(id))
                {
                    IsSequenceCorrect(id);
                    IsCitizenshipValid(_id.Citizenship);
                    IsDateFormatValid(_id.Date);
                    IsGenderTypeCorrect(_id.Gender);
                    ControlDigitTest(id);
                }

                if (_errorList.Count == 0)
                {
                    _iDbHandler.SaveUserDetails(_userDetails);
                    return new JsonResponse { ErrorLists = _errorList, UserDetails = _userDetails, Valid = true };
                }

                else
                {
                    return new JsonResponse { ErrorLists = _errorList, UserDetails = null, Valid = false };

                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
        }

        public bool IsCitizenshipValid(string Citizenship)
        {
            try
            {
                switch (Citizenship)
                {
                    case "0":
                        _userDetails.Citizenship = CitizenshipType.SouthAfrican.ToString();
                        return true;
                        
                    case "1":
                        _userDetails.Citizenship = CitizenshipType.Other.ToString();
                        return true;
                       
                    default:
                        
                        _errorList.Add(new ErrorLists { error = ErrorList.IncorrectCitizenType.ToString() });
                        return false;
                       
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
        }

        public bool IsDateFormatValid(string Date)
        {
            DateTime parsedDate;
            try
            {
                var isValidFormat = DateTime.TryParseExact(Date, DateFormat, new CultureInfo("en-US"),
                    DateTimeStyles.AdjustToUniversal, out parsedDate);
                if (!isValidFormat)
                { _errorList.Add(new ErrorLists { error = ErrorList.IncorrectDateFormat.ToString() });
                    return false;
                }
                else
                { _userDetails.DateOfBirth = parsedDate;
                    return true;
                }
                 
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
        }

        public bool IsGenderTypeCorrect(string Gender)
        {
            try
            {
                if (int.Parse(Gender) > 9999 || int.Parse(Gender) < 0)
                {
                    _errorList.Add(new ErrorLists { error = ErrorList.IncorrectGenderType.ToString() });
                    return false;
                }
                else
                {
                    _userDetails.Gender = int.Parse(Gender) < 5000 ? "Female" : "Male";
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
        }
      
        public bool IsSequenceCorrect(string id)
        {
            try
            {
                //This method is abit harder to test,Infact id say uncessecary if the check date method passes.
                //cause by default it shouldnt be hit if the date check fails.
                //Because if the date is in the wrong format the date check would fail so they would be no need to check the sequence,
                //This method will always be true. Provide the date is and gender is write
                //Iv kept it in because its one of the checks they do on ID. but its not really neccessary i think.
                BreakDownId(id);
                if (id.LastIndexOf(_id.Gender, StringComparison.Ordinal) <
                    id.LastIndexOf(_id.Date, StringComparison.Ordinal))
                {
                    _errorList.Add(new ErrorLists { error = ErrorList.IncorrectSequenceForDateOfBirth.ToString() });
                    return false;
                }
                else return true;
            }
                   
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw;
            }
        }

        // This method assumes that the 13-digit id number has 
        // valid digits in position 0 through 12.  
        // Stored in a property 'id'.  
        // Returns: the valid digit between 0 and 9, or  
        // -1 if the method fails.
        public bool ControlDigitTest(string id)
        {
            BreakDownId(id);
            var d = -1;
            try
            {
                var a = 0;
                for (var i = 0; i < 6; i++)
                {
                    a += int.Parse(id[2 * i].ToString());
                }
                var b = 0;
                for (var i = 0; i < 6; i++)
                {
                    b = b * 10 + int.Parse(id[2 * i + 1].ToString());
                }
                b *= 2;
                var c = 0;
                do
                {
                    c += b % 10;
                    b = b / 10;
                } while (b > 0);
                c += a;
                d = 10 - c % 10;
                if (d == 10) d = 0;

                if (Enumerable.Range(0, 9).Contains(d))
                {
                    if (!int.Parse(_id.ControlDigit).Equals(d))
                    {
                        _errorList.Add(new ErrorLists { error = ErrorList.IdFailedControlDigitTest.ToString() });
                        return false;
                    }
                    else return true;
                }
                else
                {
                    _errorList.Add(new ErrorLists { error = ErrorList.IncorrectLastDigit.ToString() });
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw;
            }
        }
    }
}