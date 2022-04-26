# rapl-limit-tests

This repository contains two simple system tests.

The first test is #rapl_sample_rate which serves the purpose of testing the rate at which rapl-updates are performed on the SUT (System Under Test) which is useful to be aware of when performing micro-benchmarks where the update rate of rapl can influence the results of the given benchmark.
RAPL documentation states this update rate to be 1ms, while other tests (including our own) has found a much higher frequency. 

The second test is #rapl_loop_rate which serves the purpose of veryfying that the results found with #rapl_sample_rate is accurate and that the while-loop runs fast enough that it does not miss rapl update. This is done by veryfying that the same value is read from the rapl energy usage file multiple times in a row. 

We found no significan difference between SGX enabled and disabled, but reccomend that you, the user, verify this for yourself and your system as well. 

These two system tests are written in C# and utilize the C# Stopwatch.GetTimestamp for time measurements. 
The system tests and this guide is setup to facilitate a one hour run time, while also saving the measurements in memory during runtime and writing them to a file afterwards, which means that up to 8 gigabyte of memory can be used during the runtime. 
