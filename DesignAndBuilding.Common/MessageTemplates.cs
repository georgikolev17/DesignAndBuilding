using System;
using System.Collections.Generic;
using System.Text;

namespace DesignAndBuilding.Common
{
    public static class MessageTemplates
    {
        public static string NewBidSubmitted(string buildingName, string discipline, decimal bidPrice) =>
            $"Нова оферта е подадена за обект: {buildingName}, част: {discipline} с цена {bidPrice} лв/кв.м., в който Вие също имате участие.";

        public static string NewQuestionSubmitted(string buildingName, string discipline) =>
            $"Нов въпрос е зададен относно Вашето задание по част: {discipline}, обект: : {buildingName}.";

        public static string NewAnswerSubmitted(string buildingName, string discipline) =>
            $"Получихте отговор на въпрос относно заданието по част: {discipline}, обект: : {buildingName}.";

        public static string AssignmentFinished(string buildingName, string discipline) =>
            $"Заданието по част: {discipline}, обект: {buildingName} е приключено.";

        public static string NewAssignmentCreated(string buildingName, string discipline) =>
            $"Публикувано е ново задание по част: {discipline} - обект: {buildingName}.";
    }
}
