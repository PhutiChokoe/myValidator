using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyValidatorService.Enums
{
    public enum ErrorList
    {
        IncorrectDateFormat,
        IncorrectGenderType,
        IncorrectSequenceForDateOfBirth,
        IncorrectCitizenType,
        SeceondLastNumberIncorrect,
        IncorrectLastDigit,
        InCorrectIdLength,
        IdFailedControlDigitTest

    }
}