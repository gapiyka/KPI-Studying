using SanitasLibr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanitasLibr
{
    enum ModelType
    {
        Answer,
        Schedule,
        MedCard,
        List
    }

    public class Parser
    {
        const string DAT = "|| DATE: ";
        const string DOC = "|| DOCTOR: ";
        const string DIA = "|| DIAGNOSIS: ";

        public static IEnumerable<Output> ParseStringToModel(string str)
        {
            ModelType model;
            string[] arr = str.Split(" ");
            string[] lines = str.Split("\n");
            int notesCount = lines.Length;

            if (str.Contains(DOC))
            {
                if (str.Contains(DAT))
                {
                    if (str.Contains(DIA)) model = ModelType.MedCard;
                    else model = ModelType.Schedule;
                }
                else model = ModelType.List;
            }
            else model = ModelType.Answer;
            
            switch (model)
            {
                case ModelType.Schedule:

                    return Enumerable.Range(1, notesCount-1).Select(index => new ScheduleModel
                    {
                        Date = DateTime.Parse(arr[(index - 1) * 8 + 2] + " " + arr[(index - 1) * 8 + 3] + " " + arr[(index - 1) * 8 + 4]), 
                        Doctor = arr[(index - 1) * 8 + 7].Trim('\n')
                    }).ToArray();
                    break;
                case ModelType.MedCard:
                    return Enumerable.Range(1, notesCount-1).Select(index => new MedCardModel
                    {
                        Date = DateTime.Parse(arr[(index - 1) * 11 + 2] + " " + arr[(index - 1) * 11 + 3] + " " + arr[(index - 1) * 11 + 4]),
                        Doctor = arr[(index - 1) * 11 + 7],
                        Diagnosis = arr[(index - 1) * 11 + 10].Trim('\n')
                    }).ToArray();
                    break;
                case ModelType.List:
                    return Enumerable.Range(1, notesCount-1).Select(index => new ListModel
                    {// 2 5 8 11 == 1 2 3 4 => n_i = 2+3(i-1).
                        Doctor = arr[(index - 1) * 3 + 2].Trim('\n')
                    }).ToArray();
                    break;
                default:
                    return Enumerable.Range(1, 1).Select(index => new AnswerModel
                    {
                        Result = str.Trim('\n')
                    }).ToArray();
                    break;
            }
        }
    }
}
