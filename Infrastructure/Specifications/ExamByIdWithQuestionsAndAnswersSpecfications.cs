using Core.Common.Specifications;
using Core.Entities.Exams;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class ExamByIdWithQuestionsAndAnswersSpecfications : BaseSpecification<Exam>
    {
        public ExamByIdWithQuestionsAndAnswersSpecfications(Guid Id): base(e => e.Id == Id)
        {
            AddInclude(e => e.ExamQuestions);
            AddInclude($"{nameof(Exam.ExamQuestions)}.{nameof(ExamQuestions.Question)}");
            //AddInclude(e => e.ExamQuestions.Select(eq => eq.Question.AnswerOptions));
            AddInclude($"{nameof(Exam.ExamQuestions)}.{nameof(ExamQuestions.Question)}.{nameof(Question.AnswerOptions)}");

        }
    }
}
