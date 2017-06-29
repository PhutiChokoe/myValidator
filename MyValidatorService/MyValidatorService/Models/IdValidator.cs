using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using MyValidatorService.Enums;
using NLog;

namespace MyValidatorService.Models
{
    public class IdValidator
    {
        private const string DateFormat = "yyMMdd";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly List<string> _errorList = new List<string>();
        private readonly IdBreakDown _id = new IdBreakDown();
        private readonly UserDetails _userDetails = new UserDetails();


        public bool IsIdLengthValid(string id)
        {
            try
            {
                if (id.Length != 13)
                {
                    _errorList.Add(ErrorList.InCorrectIdLength.ToString());
                    return false;
                }

                else
                {
                    _userDetails.IdNumber = long.Parse(id);
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
            if (IsIdLengthValid(id))
            {
                IsSequenceCorrect(id);
                IsCitizenshipValid();
                IsDateFormatValid();
                IsGenderTypeCorrect();
                ControlDigitTest(id);
            }

            if (_errorList.Count == 0)
                return new JsonResponse { ErrorLists = _errorList, UserDetails = _userDetails, Valid = true };
            else
                return new JsonResponse { ErrorLists = _errorList, UserDetails = null, Valid = false };
        }

        public void IsCitizenshipValid()
        {
            try
            {
                switch (_id.Citizenship)
                {
                    case "0":
                        _userDetails.Citizenship = CitizenshipType.SouthAfrican.ToString();
                        break;
                    case "1":
                        _userDetails.Citizenship = CitizenshipType.Other.ToString();
                        break;
                    default:
                        _errorList.Add(ErrorList.IncorrectCitizenType.ToString());
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
        }

        public void IsDateFormatValid()
        {
            DateTime parsedDate;
            try
            {
                var isValidFormat = DateTime.TryParseExact(_id.Date, DateFormat, new CultureInfo("en-US"),
                    DateTimeStyles.AdjustToUniversal, out parsedDate);
                if (!isValidFormat)
                    _errorList.Add(ErrorList.IncorrectDateFormat.ToString());
                else
                    _userDetails.DateOfBirth = parsedDate.ToShortDateString();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
        }

        public void IsGenderTypeCorrect()
        {
            try
            {
                if (int.Parse(_id.Gender) > 9999 && int.Parse(_id.Gender) < 0)
                    _errorList.Add(ErrorList.IncorrectGenderType.ToString());
                else
                    _userDetails.Gender = int.Parse(_id.Gender) < 5000 ? "Female" : "Male";
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
        }

        public void IsSequenceCorrect(string id)
        {
            try
            {
                if (id.LastIndexOf(_id.Gender, StringComparison.Ordinal) <
                    id.LastIndexOf(_id.Date, StringComparison.Ordinal))
                    _errorList.Add(ErrorList.IncorrectSequenceForDateOfBirth.ToString());
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
        // from http://geekswithblogs.net/willemf/archive/2005/10/30/58561.aspx
        public void ControlDigitTest(string id)
        {
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
                        _errorList.Add(ErrorList.IdFailedControlDigitTest.ToString());
                    }
                }
                else
                {
                    _errorList.Add(ErrorList.IncorrectLastDigit.ToString());
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