open System
open weka.core
open weka.classifiers
open System.Diagnostics
open java

try
  let args = System.Environment.GetCommandLineArgs()
  let runs = if args.Length>1 then int32(args.[1]) else 1
  let algo = if args.Length>2 then args.[2] else ""
  let data = if args.Length>3 then args.[3] else ""
  
  let read = new Stopwatch()
  let build = new Stopwatch()
  let classify = new Stopwatch()
  for cnt = 1 to runs do
    read.Start();
    let train = new Instances(new java.io.FileReader(data+"train.arff"))
    train.setClassIndex(train.numAttributes() - 1)
    let test = new Instances(new java.io.FileReader(data+"test.arff"))
    test.setClassIndex(test.numAttributes() - 1)
    read.Stop()

    let clList : Classifier list = [
      new weka.classifiers.bayes.NaiveBayes();
      new weka.classifiers.trees.RandomForest();
      new weka.classifiers.trees.J48();
      new weka.classifiers.functions.MultilayerPerceptron();
      new weka.classifiers.rules.ConjunctiveRule();
      new weka.classifiers.functions.SMO()
    ]
    
    build.Start()
    for classifier in clList do
      if algo.Equals("") || algo.Equals("All") || classifier.getClass().getSimpleName().Equals(algo) then
        classifier.buildClassifier(train)
    build.Stop()

    classify.Start()
    for classifier in clList do
      if algo.Equals("") || algo.Equals("All") || classifier.getClass().getSimpleName().Equals(algo) then
        let mutable numCorrect = 0
        for i = 0 to test.numInstances()-1 do
          if classifier.classifyInstance(test.instance(i)) = test.instance(i).classValue() then
            numCorrect <- numCorrect + 1
        //printfn "%s\t%d%s%d%s%f%s" (classifier.getClass().getSimpleName()) numCorrect " out of "  (test.numInstances()) " correct (" ((100.0 * float(numCorrect)) / float(test.numInstances())) "%)"
    classify.Stop()
  printfn "{\"%s\",%d,%d,%d,%d};" algo read.ElapsedMilliseconds build.ElapsedMilliseconds classify.ElapsedMilliseconds (read.ElapsedMilliseconds+build.ElapsedMilliseconds+classify.ElapsedMilliseconds)
  if args.Length>4 then Console.ReadLine() |> ignore
with
  | :? java.lang.Exception as e -> e.printStackTrace()