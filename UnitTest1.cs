namespace GossipGame;

using System;
using System.Linq;
using FluentAssertions;
public class UnitTest1
{
    public bool stops { get; private set; }

    [Fact]
    public void DontGossip()
    {
        StopBluePrint stopBluePrint = new StopBluePrint
        {
            stops = new List<Stop>
            {
                new Stop{ points = new List<int>{ 1 } },
                new Stop{ points = new List<int>{ 2 } },
            }
        };

        var max = 0;
        State state = new State
        {
            drivers = new List<DriverState>{
                new DriverState { id = 1, gossip=1 },
                new DriverState { id = 2, gossip=1 },
            }
        };
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        GossipService.Check(state, stopBluePrint, max, keyValues);
        state.drivers[0].gossip.Should().Be(1);
        state.drivers[1].gossip.Should().Be(1);
    }

    [Fact]
    public void AbleToGossip()
    {
        StopBluePrint stopBluePrint = new StopBluePrint
        {
            stops = new List<Stop>
            {
                new Stop{ points = new List<int>{ 2 } },
                new Stop{ points = new List<int>{ 2 } }
            }
        };

        var max = 0;
        State state = new State
        {
            drivers = new List<DriverState>{
                new DriverState { id= 1, gossip=1},
                new DriverState { id= 2, gossip=1},
            }
        };
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        GossipService.Check(state, stopBluePrint, max, keyValues);
        state.drivers[0].gossip.Should().Be(2);
        state.drivers[1].gossip.Should().Be(2);
    }

    [Fact]
    public void TwoAbleToGossip()
    {
        StopBluePrint stopBluePrint = new StopBluePrint
        {
            stops = new List<Stop>
            {
                new Stop{ points = new List<int>{ 3 } },
                new Stop{ points = new List<int>{ 2 } },
                new Stop{ points = new List<int>{ 3 } }
            }
        };

        var max = 0;
        State state = new State
        {
            drivers = new List<DriverState>{
                new DriverState { id= 1, gossip=1},
                new DriverState { id= 2, gossip=1},
                new DriverState { id= 3, gossip=1},
            }
        };
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        GossipService.Check(state, stopBluePrint, max, keyValues);
        state.drivers[0].gossip.Should().Be(2);
        state.drivers[1].gossip.Should().Be(1);
        state.drivers[2].gossip.Should().Be(2);
    }

    [Fact]
    public void GossipInLoop()
    {
        StopBluePrint stopBluePrint = new StopBluePrint
        {
            stops = new List<Stop>
            {
                new Stop{ points = new List<int>{ 3, 1 } },
                new Stop{ points = new List<int>{ 2, 1 } },
                new Stop{ points = new List<int>{ 3, 2 } }
            }
        };

        var max = 0;
        State state = new State
        {
            drivers = new List<DriverState>{
                new DriverState { id= 1, gossip=1},
                new DriverState { id= 2, gossip=1},
                new DriverState { id= 3, gossip=1},
            }
        };
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        GossipService.Check(state, stopBluePrint, max, keyValues);
        state.drivers[0].gossip.Should().Be(3);
        state.drivers[1].gossip.Should().Be(2);
        state.drivers[2].gossip.Should().Be(2);
    }

    [Fact]
    public void IgnoreOverLapGossip()
    {
        StopBluePrint stopBluePrint = new StopBluePrint
        {
            stops = new List<Stop>
            {
                new Stop{ points = new List<int>{ 1,2} },
                new Stop{ points = new List<int>{ 1,2} },
            }
        };
        var max = 0;
        State state = new State
        {
            drivers = new List<DriverState>{
                new DriverState { id=1,gossip= 1},
                new DriverState { id=2,gossip= 1},
            }
        };
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        GossipService.Check(state, stopBluePrint, max, keyValues);
        state.drivers[0].gossip.Should().Be(2);
        state.drivers[0].gossip.Should().Be(2);
    }

    [Fact]
    public void AllGossipHasSpread()
    {
        StopBluePrint stopBluePrint = new StopBluePrint
        {
            stops = new List<Stop>
            {
                new Stop{ points = new List<int>{ 1,3,4} },
                new Stop{ points = new List<int>{ 1,2,3} },
                new Stop{ points = new List<int>{ 3,2,4} }
            }
        };
        var max = 0;
        State state = new State
        {
            drivers = new List<DriverState>{
                new DriverState { id= 1, gossip=1},
                new DriverState { id= 2, gossip=1},
                new DriverState { id= 3, gossip=1},
            }
        };
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        GossipService.Check(state, stopBluePrint, max, keyValues);
        state.drivers[0].gossip.Should().Be(3);
        state.drivers[1].gossip.Should().Be(3);
        state.drivers[2].gossip.Should().Be(3);
    }

    [Fact]
    public void AllGossipAtTheFirstStop()
    {
        StopBluePrint stopBluePrint = new StopBluePrint
        {
            stops = new List<Stop>
            {
                new Stop{ points = new List<int>{ 1 } },
                new Stop{ points = new List<int>{ 1 } },
                new Stop{ points = new List<int>{ 1 } }
            }
        };
        var max = 0;
        State state = new State
        {
            drivers = new List<DriverState>{
                new DriverState { id= 1, gossip=1},
                new DriverState { id= 2, gossip=1},
                new DriverState { id= 3, gossip=1},
            }
        };
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        GossipService.Check(state, stopBluePrint, max, keyValues);
        state.drivers[0].gossip.Should().Be(3);
        state.drivers[1].gossip.Should().Be(3);
        state.drivers[2].gossip.Should().Be(3);
    }

    [Fact]
    public void AllGossipAtFirstStopWhatHappensAtNextStop()
    {
        StopBluePrint stopBluePrint = new StopBluePrint
        {
            stops = new List<Stop>
            {
                new Stop{ points = new List<int>{ 1,2,3,5 } },
                new Stop{ points = new List<int>{ 1,2,4,3 } },
                new Stop{ points = new List<int>{ 1,3,4,5 } }
            }
        };
        var max = 0;
        State state = new State
        {
            drivers = new List<DriverState>{
                new DriverState { id= 1, gossip=1},
                new DriverState { id= 2, gossip=1},
                new DriverState { id= 3, gossip=1},
            }
        };
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        GossipService.Check(state, stopBluePrint, max, keyValues);
        state.drivers[0].gossip.Should().Be(3);
        state.drivers[1].gossip.Should().Be(3);
        state.drivers[2].gossip.Should().Be(3);
    }
}


internal class Stop
{
    public Stop()
    {
    }

    public List<int> points { get; internal set; }
}

internal class StopBluePrint
{
    public List<Stop> stops { get; set; }
}

internal class GossipService
{
    internal static void Check(State state, StopBluePrint stopBluePrint, int max, Dictionary<string, int> keyValues)
    {
        if (max == 3)
        {
            return;
        }
        state.drivers.ForEach(d => d.stop ??= stopBluePrint.stops[d.id-1].points[max%stopBluePrint.stops[d.id - 1].points.Count]);
        var c = state.drivers.GroupBy(d => d.stop);
        var count = 0;
        max++;
        foreach(var d in c)
        {
            List<DriverState> drivers = state.drivers.FindAll(driver => driver.stop == d.Key);
            if (d.Count() > 1)
            {
                var ids = drivers.Select(d => d.id).ToList<int>();
                if (d.Count()>2)
                {
                    List<List<int>> splitArray = new List<List<int>>();
                    for (int i = 0; i < d.Count(); i++)
                    {
                        List<int> ints = new List<int> { ids[i], ids[(i + 1) % d.Count()] };
                        string key = string.Join(":", ints);
                        if (tryKeyValue(keyValues, key)) return;
                        splitArray.Add(ints);
                    }
                } else
                {
                    string key = string.Join(":",ids);
                    if (tryKeyValue(keyValues, key)) return;
                }
                drivers.ForEach(dvr => dvr.gossip += d.Count() - 1);
            }
            drivers.ForEach(dvr => dvr.stop = stopBluePrint.stops[dvr.id - 1].points[max%stopBluePrint.stops[dvr.id -1].points.Count]);
        }
        Check(state, stopBluePrint, max, keyValues);
        return;
    }

    private static bool tryKeyValue(Dictionary<string, int> keyValues, string key)
    {
        int a;
        keyValues.TryGetValue(key, out a);
        if (a == 1)
        {
            return true;
        }
        keyValues[key] = 1;
        return false;
    }
}

internal record State
{
    public List<DriverState> drivers { get; set; }
}

public class DriverState
{
    public int? stop { get; internal set; }
    public int gossip { get; internal set; }
    public int id { get; internal set; }
}