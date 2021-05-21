# InBetweenNetGeneration

## Table of contents

* [Introduction](#introduction)
* [Input](#input)
* [Output](#output)

## Introduction

This directory contains the data files which have been used in running the application. The source of each input file is presented in the table below.

Data | Type | Set | Source
:--- | :---: | :---: | :---:
DrugBank | Drug-Target | Upstream | [Link](https://doi.org/10.1093/nar/gkx1037)

## Input

The ``Input`` directory contains the input data for the application. The sets of downstream nodes can be found in the text files named ``Source_Downstream``, while the sets of upstream nodes can be found in the text files named ``Source_Upstream``.

## Output

The ``Output`` directory contains the networks obtained after running the application once on each group of one set of downstream nodes and one set of upstream nodes in the ``Input`` directory. Each output consists of a text file named ``Source_Downstream_Source_Upstream_Output``.
