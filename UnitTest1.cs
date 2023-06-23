namespace GossipGame;

using System;
using FluentAssertions;
using System.Text.Json;
using System.Text;

public class UnitTest1
{
    [Fact]
    public void DontGossip()
    {
        StopBluePrint stopBluePrint = new StopBluePrint()
                                        .addStops(new List<int> { 1 })
                                        .addStops(new List<int> { 2 });
        State state = new State(minutesDriven: 0).addDriver(1, 1).addDriver(2, 1);
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        Dictionary<string, byte> stateCache = new Dictionary<string, byte>();
        GossipService.Check(state, stopBluePrint, keyValues, stateCache);
        state.drivers[0].gossip.Should().Be(1);
        state.drivers[1].gossip.Should().Be(1);
    }

    [Fact]
    public void AbleToGossip()
    {
        StopBluePrint stopBluePrint = new StopBluePrint().addStops(new List<int> { 2 }).addStops(new List<int> { 2 });
        State state = new State(minutesDriven: 0).addDriver(1, 1).addDriver(2, 1);
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        Dictionary<string, byte> stateCache = new Dictionary<string, byte>();
        GossipService.Check(state, stopBluePrint, keyValues, stateCache);
        state.drivers[0].gossip.Should().Be(2);
        state.drivers[1].gossip.Should().Be(2);
    }

    [Fact]
    public void TwoAbleToGossip()
    {
        StopBluePrint stopBluePrint = new StopBluePrint().addStops(new List<int> { 3 })
            .addStops(new List<int> { 2 }).addStops(new List<int> { 3 });
        State state = new State(minutesDriven: 0).addDriver(1, 1).addDriver(2, 1).addDriver(3, 1);
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        Dictionary<string, byte> stateCache = new Dictionary<string, byte>();
        GossipService.Check(state, stopBluePrint, keyValues, stateCache);
        state.drivers[0].gossip.Should().Be(2);
        state.drivers[1].gossip.Should().Be(1);
        state.drivers[2].gossip.Should().Be(2);
    }

    [Fact]
    public void GossipInLoop()
    {
        StopBluePrint stopBluePrint = new StopBluePrint().addStops(new List<int> { 3, 1 })
                                        .addStops(new List<int> { 2, 1 }).addStops(new List<int> { 3, 2 });
        State state = new State(minutesDriven: 0).addDriver(1, 1).addDriver(2, 1).addDriver(3, 1);
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        Dictionary<string, byte> stateCache = new Dictionary<string, byte>();
        GossipService.Check(state, stopBluePrint, keyValues, stateCache);
        state.drivers[0].gossip.Should().Be(3);
        state.drivers[1].gossip.Should().Be(2);
        state.drivers[2].gossip.Should().Be(2);
    }

    [Fact]
    public void IgnoreOverLapGossip()
    {
        StopBluePrint stopBluePrint = new StopBluePrint().addStops(new List<int> { 1, 2 }).addStops(new List<int> { 1, 2 });
        State state = new State(minutesDriven: 0).addDriver(1, 1).addDriver(2, 1);
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        Dictionary<string, byte> stateCache = new Dictionary<string, byte>();
        GossipService.Check(state, stopBluePrint, keyValues, stateCache);
        state.drivers[0].gossip.Should().Be(2);
        state.drivers[0].gossip.Should().Be(2);
    }

    [Fact]
    public void AllGossipHasSpread()
    {
        StopBluePrint stopBluePrint = new StopBluePrint().addStops(new List<int> { 1, 3, 4 })
                                        .addStops(new List<int> { 1, 2, 3 })
                                        .addStops(new List<int> { 3, 2, 4 });
        State state = new State(minutesDriven: 0).addDriver(1, 1).addDriver(2, 1).addDriver(3, 1);
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        Dictionary<string, byte> stateCache = new Dictionary<string, byte>();
        GossipService.Check(state, stopBluePrint, keyValues, stateCache);
        state.drivers[0].gossip.Should().Be(3);
        state.drivers[1].gossip.Should().Be(3);
        state.drivers[2].gossip.Should().Be(3);
    }

    [Fact]
    public void AllGossipAtTheFirstStop()
    {
        StopBluePrint stopBluePrint = new StopBluePrint().addStops(new List<int> { 1 }).addStops(new List<int> { 1 }).addStops(new List<int> { 1 });
        State state = new State(minutesDriven: 0).addDriver(1, 1).addDriver(2, 1).addDriver(3, 1);
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        Dictionary<string, byte> stateCache = new Dictionary<string, byte>();
        GossipService.Check(state, stopBluePrint, keyValues, stateCache);
        state.drivers[0].gossip.Should().Be(3);
        state.drivers[1].gossip.Should().Be(3);
        state.drivers[2].gossip.Should().Be(3);
    }

    [Fact]
    public void AllGossipAtFirstStopWhatHappensAtNextStop()
    {
        StopBluePrint stopBluePrint = new StopBluePrint().addStops(new List<int> { 1, 2, 3, 5 })
                                                .addStops(new List<int> { 1, 2, 4, 3 })
                                                .addStops(new List<int> { 1, 3, 4, 5 });
        State state = new State( minutesDriven: 0 ).addDriver(1, 1).addDriver(2, 1).addDriver(3, 1);
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        Dictionary<string, byte> stateCache = new Dictionary<string, byte>();
        GossipService.Check(state, stopBluePrint, keyValues, stateCache);
        state.drivers[0].gossip.Should().Be(3);
        state.drivers[1].gossip.Should().Be(3);
        state.drivers[2].gossip.Should().Be(3);
    }

    [Fact]
    public void CheckWhenGossipHasTotallySpread()
    {
        StopBluePrint stopBluePrint = new StopBluePrint().addStops(new List<int> { 3, 1, 2, 3 })
                                        .addStops(new List<int> { 3, 2, 3, 1 })
                                        .addStops(new List<int> { 4, 2, 3, 4, 5 });
        State state = new State(minutesDriven: 0)
            .addDriver(id: 1, gossip: 1)
            .addDriver(id: 2, gossip: 1)
            .addDriver(id: 3, gossip: 1);
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        Dictionary<string, byte> stateCache = new Dictionary<string, byte>();
        GossipService.Check(state, stopBluePrint, keyValues, stateCache,8 * 60);
        state.minutesDriven.Should().Be(6);

    }

    [Fact]
    public void DetechCircularStop()
    {
        StopBluePrint stopBluePrint = new StopBluePrint().addStops(new List<int> { 2, 1, 2 })
            .addStops(new List<int> { 5, 2, 8 });
        State state = new State(minutesDriven: 0).addDriver(1, 1).addDriver(2, 1);
        Dictionary<string, int> keyValues = new Dictionary<string, int>();
        // This is sthe stateCache to check if the current state was reached before
        // If a cache hit here means we are just going in circle and no point continuing
        Dictionary<string, byte> stateCache = new Dictionary<string, byte>();
        GossipService.Check(state, stopBluePrint, keyValues, stateCache, 8 * 60);
        // It should detect that there is no point calculating as its impossible for the drivers to gossip
        // with this route combination
        state.minutesDriven.Should().Be(4);
    }

    internal class GossipService
    {
        // This method is too convulated to my liking
        // Its just some dynamic programming (recursion & caching)
        internal static void Check(State state, StopBluePrint stopBluePrint, Dictionary<string, int> driverGossipHistory, Dictionary<string, byte> stateCache, int workHours = 4)
        {
            if (stateCache.TryGetValue(state.getKey(), out _))
            {
                return;
            }

            stateCache[state.getKey()] = 1;
            if (state.minutesDriven == workHours)
            {
                return;
            }
            state.drivers.ForEach(d => d.stop ??= stopBluePrint.stops[d.id - 1].points[state.minutesDriven % stopBluePrint.stops[d.id - 1].points.Count]);
            var c = state.drivers.GroupBy(d => d.stop);
            foreach (var d in c)
            {
                List<State.DriverState> drivers = state.drivers.FindAll(driver => driver.stop == d.Key);
                if (d.Count() > 1)
                {
                    var ids = drivers.Select(d => d.id).ToList<int>();
                    bool t = false;
                    if (d.Count() > 2)
                    {
                        List<List<int>> splitArray = new List<List<int>>();
                        for (int i = 0; i < d.Count(); i++)
                        {
                            List<int> ints = new List<int> { ids[i], ids[(i + 1) % d.Count()] };
                            string key = string.Join(":", ints);
                            t = tryKeyValue(driverGossipHistory, key);
                            splitArray.Add(ints);
                        }
                    }
                    else
                    {
                        string key = string.Join(":", ids);
                        t = tryKeyValue(driverGossipHistory, key);
                    }
                    if (!t) drivers.ForEach(dvr => dvr.gossip += d.Count() - 1);
                }
                // If all the drivers have a cache hit against all other drivers
                // There is no point continuing as they have all gossipped with each other
                if (driverGossipHistory.Keys.Count()==state.drivers.Count)
                {
                    return;
                }
                // This step is to go to the next stop
                drivers.ForEach(dvr => dvr.stop = stopBluePrint.stops[dvr.id - 1].points[(state.minutesDriven+1) % stopBluePrint.stops[dvr.id - 1].points.Count]);
            }
            state.minutesDriven += 1;
            Check(state, stopBluePrint, driverGossipHistory, stateCache, workHours);
            return;
        }

        private static bool tryKeyValue(Dictionary<string, int> driverGossipHistory, string key)
        {
            int a;
            driverGossipHistory.TryGetValue(key, out a);
            if (a == 1)
            {
                return true;
            }
            driverGossipHistory[key] = 1;
            return false;
        }
    }

    // State of the Game
    internal record State
    {
        public State(int minutesDriven)
        {
            this.minutesDriven = minutesDriven;
        }

        public string getKey()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var driver in drivers)
            {
                stringBuilder.Append(driver.stop).Append(':');
            }
            return stringBuilder.ToString();
        }

        public List<DriverState> drivers { get; set; }
        public int minutesDriven { get; set; }
        public int gossipsSpread { get; set; }

        public State addDriver(int id, int gossip)
        {
            this.drivers ??= new List<DriverState>();
            this.drivers.Add(new DriverState
            {
                id = id,
                gossip = gossip,
            });
            return this;
        }

        // State of the Driver
        public class DriverState
        {
            public int? stop { get; internal set; }
            public int gossip { get; internal set; }
            public int id { get; internal set; }
        }
    }

    // BluePrint for the game
    internal class StopBluePrint
    {
        public StopBluePrint addStops(List<int> stop_list)
        {
            this.stops ??= new List<Stop>();
            this.stops.Add(new Stop { points = stop_list });
            return this;
        }
        public List<Stop> stops { get; set; }
        internal class Stop
        {
            public Stop()
            {
            }

            public List<int> points { get; internal set; }
        }
    }
}