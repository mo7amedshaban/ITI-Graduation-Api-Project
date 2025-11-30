using Core.Common.Specifications;
using Core.Entities.Exams;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class ExamByIdWithQuestionsSpecfications : BaseSpecification<Exam>
    {
        public ExamByIdWithQuestionsSpecfications(Guid ExamId):base(e => e.Id == ExamId)
        {
           
            AddInclude(e => e.ExamQuestions);
            AddInclude($"{nameof(Exam.ExamQuestions)}.{nameof(ExamQuestions.Question)}");
            //AddInclude(e => e.ExamQuestions.Select(eq => eq.Question.AnswerOptions));
          



        }
    }
}
