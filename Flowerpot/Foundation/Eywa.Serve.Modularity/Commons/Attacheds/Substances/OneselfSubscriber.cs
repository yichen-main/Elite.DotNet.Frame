namespace Eywa.Serve.Modularity.Commons.Attacheds.Substances;
public class OneselfSubscriber
{
    [Subscribe, Topic(nameof(TestKanban))]
    public IEnumerable<TestKanban> OnTestKanban([EventMessage] IEnumerable<TestKanban> infos)
    {
        return infos;
    }
}