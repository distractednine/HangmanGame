namespace HangmanGame.UnitTests.TestData
{
    public class ServerResponses
    {
        public static string WordAssociationsJsonResponse = @"
{
    'version': '1.0',
    'code': 200,
    'request': {
        'text': [
            'hobby'
        ],
        'lang': 'en',
        'type': 'stimulus',
        'limit': 50,
        'pos': 'noun',
        'indent': 'yes'
    },
    'response': [
        {
            'text': 'hobby',
            'items': [
                {
                    'item': 'Gardening',
                    'weight': 100,
                    'pos': 'noun'
                },
                {
                    'item': 'Collecting',
                    'weight': 97,
                    'pos': 'noun'
                },
                {
                    'item': 'Cooking',
                    'weight': 80,
                    'pos': 'noun'
                }
            ]
        }
    ]
}
".Replace("'", "\"");
    }
}
