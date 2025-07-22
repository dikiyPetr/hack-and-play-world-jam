using VContainer;
using VContainer.Unity;

public class GamePresenter: IStartable
{
    private readonly HelloWorldService _helloWorldService;
    private readonly HelloScreen _helloScreen;

    public GamePresenter(HelloWorldService helloWorldService, HelloScreen helloScreen)
    {
        _helloWorldService = helloWorldService;
        _helloScreen = helloScreen;
    }

    public void Start()
    {
        _helloScreen.HelloButton.onClick.AddListener(() => _helloWorldService.Hello());
    }
}