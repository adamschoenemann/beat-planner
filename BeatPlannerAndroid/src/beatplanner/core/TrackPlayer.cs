using System;
using System.Threading;

namespace BeatPlanner
{
  public class TrackPlayer
  {
    private Metronome metro;
    private bool shouldPlay = false;

    public int Index { get; set; }

    public readonly Track Track;

    public TrackPlayer(Track track, Metronome metro)
    {
      this.Track = track;
      this.metro = metro;
    }

    public TrackPlayer(Track track) : this(track, new Metronome())
    {

    }

    public void PlayAsync()
    {
      Thread thread = new Thread(Play);
      thread.Start();
    }

    public void Stop()
    {
      shouldPlay = false;
    }

    public void Reset()
    {
      Index = 0;
    }

    public void Play()
    {
      shouldPlay = true;
      Sequence[] sequences = Track.Sequences;
      Sequence seq = sequences[Index];
      Beat beat = seq.Beat;
      int reps = seq.Reps;
      int nextChange = reps;
      metro.Beat = beat;
      metro.Start();
      while (shouldPlay)
      {
        if (metro.Bars == nextChange)
        {
          if (Index == sequences.Length - 1)
            break;
          seq = sequences[++Index];
          beat = seq.Beat;
          reps = seq.Reps;
          metro.Beat = beat;
          nextChange += reps;
        }
      }
      metro.Stop();
    }
  }
}

