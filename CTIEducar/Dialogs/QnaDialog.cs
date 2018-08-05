using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace CTIEducar.Dialogs
{
    [Serializable]
    public class QnaDialog : QnAMakerDialog
    {
        public QnaDialog(bool convBegan = false) : base(new QnAMakerService( new QnAMakerAttribute(ConfigurationManager.AppSettings["QnaSubscriptionKey"], ConfigurationManager.AppSettings["QnaKnowledgebaseId"], "Não encontramos sua resposta. Por favor entre em contato pelo nosso WhatsApp: (11) 99335-6454. Estamos à disposição.", 0.5)))
        {

        }

        private static void AddHeroCardToAnswer(Activity answer, string[] answerData)
        {
            var answerTitle = answerData[0];
            var answerDesc = answerData[1];
            var answerURL = answerData[2];
            var answerImageURL = answerData[3];

            HeroCard card = new HeroCard { Title = answerTitle, Subtitle = answerDesc, };
            card.Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Visite nosso site.", value: answerURL) };
            card.Images = new List<CardImage> { new CardImage(answerURL = answerImageURL, answerTitle) };
            answer.Attachments.Add(card.ToAttachment());
        }

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var firstAnswer = result.Answers.First().Answer;
            Activity answer = ((Activity)context.Activity).CreateReply();

            var answerPipeData = firstAnswer.Split('|');

            if (answerPipeData.Length == 1)
            {
                var answerData = firstAnswer.Split(';');
                if (answerData.Length == 1)
                {
                    await context.PostAsync(firstAnswer);
                    return;
                }

                AddHeroCardToAnswer(answer, answerData);
            }
            else
            {
                answer.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                foreach (string itemAnswerData in answerPipeData)
                {
                    var answerData = itemAnswerData.Split(';');
					if(answerData.Length >= 4)
						AddHeroCardToAnswer(answer, answerData);
                }
            }

            await context.PostAsync(answer);
        }
    }
}