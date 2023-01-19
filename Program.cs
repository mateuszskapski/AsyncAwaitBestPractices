  //await DemoRunner.SlowVsFastBreakfast();

  //var breakfastMaker = new BreakfastMaker();
  //breakfastMaker.StartDevice_OnClick(breakfastMaker, new EventArgs());

// await DemoRunner.CancellationTokenDemo();

await new TaskDemos().WhenAllDemo(100, TimeSpan.FromSeconds(10));