using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Node
{
    #region FIELDS

    public Node ParentNode;
    public Node Child;
    public List<Node> ChildNodes = new List<Node>();
    public int Visits;
    public GameStateRep GameStateRep;
    public GameStateRep NewGameStateRep;
    public Unit ChosenUnit;
    public int ChosenAction;
    public int2 moveAction, AttackAction;
    public float Value;
    public static double Difficulty;

    public bool IsRoot => ParentNode == null;
    public bool IsLeaf => ChildNodes.Count == 0;

    #endregion FIELDS

    #region METHODS

    public Node(GameStateRep StateRep, Node parent = null, Unit chosenUnit = null, int chosenAction = 0, float value = 0)
    {
        GameStateRep = StateRep;
        ParentNode = parent;
        ChosenUnit = chosenUnit;
        ChosenAction = chosenAction;
        Value = value;
        Visits = 0;
    }

    public static Node TheFullMonte(GameStateRep GState)
    {
        Node node = new Node(GState);
        CreateChildNodes(node);
        while (node.Visits < 5)
        {
            //Debug.Log(node.Visits);
            Node promisingNode = SelectPromisingNode(node);
            /*Debug.Log("Chosen Action : " + promisingNode.ChosenAction);
            Debug.Log("Move Action : " + promisingNode.moveAction);
            Debug.Log("Attack Action : " + promisingNode.AttackAction);
            Debug.Log("Promising Node Value : " + promisingNode.Value);*/
            if (promisingNode.GameStateRep.GameOver)
            {
                continue;
            }
            //ExpandNode(promisingNode);

            Node nodeToExplore = promisingNode;
            if (promisingNode.ChildNodes.Count > 0)
            {
                var random = new System.Random();
                nodeToExplore = promisingNode.ChildNodes[random.Next(0, promisingNode.ChildNodes.Count)];
            }
            float playoutResult = SimulateRandomPlayout(nodeToExplore);
            BackPropogation(nodeToExplore, playoutResult);
        }
        //Debug.Log("Chosen Action : " + node.ChosenAction);
        //Debug.Log("Move Action : " + node.moveAction);
        //Debug.Log("Attack Action : " + node.AttackAction);
        return SelectPromisingNode(node);
    }

    private static void BackPropogation(Node nodeToExplore, float playoutResult)
    {
        nodeToExplore.Visits++;
        nodeToExplore.Value += playoutResult;
        if (nodeToExplore.ParentNode != null & !nodeToExplore.IsRoot)
        {
            BackPropogation(nodeToExplore.ParentNode, playoutResult);
        }
    }

    private static float SimulateRandomPlayout(Node nodeToExplore)
    {
        return nodeToExplore.GameStateRep.GetPlayerValue(2);
    }

    private static void ExpandNode(Node promisingNode)
    {
        //Debug.Log(promisingNode.GameStateRep.Player2Units.Count);
        promisingNode.ChosenUnit = promisingNode.GameStateRep.Player2Units[UnityEngine.Random.Range(0, promisingNode.GameStateRep.Player2Units.Count - 1)];

        List<int2> moves = promisingNode.ChosenUnit.tileManager.GetSelectableTilesForMove(promisingNode.ChosenUnit.unitProperties.Pos, promisingNode.ChosenUnit.unitProperties.attackRange);
        List<int2> attacks = promisingNode.ChosenUnit.tileManager.GetSelectableTilesForAttack(promisingNode.ChosenUnit.unitProperties.Pos, promisingNode.ChosenUnit.unitProperties.attackRange);
        //Debug.Log(moves.Count);
        //Debug.Log(attacks.Count);
        int action = UnityEngine.Random.Range(1, 100);
        if (action < 30 & moves.Count > 0)
        {
            promisingNode.ChosenAction = 1;
        }
        else if ((action >= 30 & attacks.Count > 0) | attacks.Count > 0)
        {
            promisingNode.ChosenAction = 2;
        }
        promisingNode.NewGameStateRep = promisingNode.GameStateRep;
        if (promisingNode.ChosenAction == 1 & moves.Count > 0)
        {
            promisingNode.moveAction = moves[UnityEngine.Random.Range(0, moves.Count)];
            //promisingNode.NewGameStateRep = promisingNode.GameStateRep;
            /*foreach (Unit unit in promisingNode.NewGameStateRep.Player2Units)
            {
                if (unit.unitProperties.Pos.Equals(promisingNode.moveAction))
                {
                    unit.unitProperties.Pos = promisingNode.moveAction;
                }
            }*/
        }
        if (promisingNode.ChosenAction == 2 & attacks.Count > 0)
        {
            promisingNode.AttackAction = attacks[UnityEngine.Random.Range(0, attacks.Count)];
            promisingNode.Value *= 5;
            //Debug.Log("Attack Action : " + promisingNode.AttackAction);
            //promisingNode.NewGameStateRep = promisingNode.GameStateRep;
            /*foreach (Unit unit in promisingNode.NewGameStateRep.Player1Units)
            {
                if (unit.unitProperties.Pos.Equals(promisingNode.AttackAction))
                {
                    unit.unitProperties.health -= promisingNode.ChosenUnit.unitProperties.attack;
                }
            }*/
        }
    }

    public static void CreateChildNodes(Node parentN)
    {
        foreach (Unit unit in parentN.GameStateRep.Player2Units)
        {
            Node child = new Node(parentN.GameStateRep, parentN, unit);
            ExpandNode(child);
            child.Value = child.NewGameStateRep.GetPlayerValue(2);
            parentN.ChildNodes.Add(child);
        }
    }

    private static Node SelectPromisingNode(Node node)
    {
        //CreateChildNodes(node);
        List<float> ChildVal = new List<float>();
        foreach (Node child in node.ChildNodes)
        {
            //UCB1
            ChildVal.Add((int)(child.Value / child.Visits) + (float)Difficulty * Mathf.Sqrt(2 * Mathf.Log(node.Visits) / child.Visits));
            //Debug.Log("Value : " + child.Value);
        }
        int index = 0;
        float max = 0;
        for (int i = 0; i < ChildVal.Count; i++)
        {
            if (ChildVal[i] >= max)
            {
                if (ChildVal[i] == max)
                {
                    if (UnityEngine.Random.Range(0, 100) % 2 == 0)
                    {
                        max = ChildVal[i];
                        index = i;
                    }
                }
                else
                {
                    max = ChildVal[i];
                    index = i;
                }
            }
        }
        return node.ChildNodes[index];
    }

    public void AddChild(Node child)
    {
        ChildNodes.Add(child);
        child.ParentNode = this;
    }

    #endregion METHODS
}