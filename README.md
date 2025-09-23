<h2>♻️ Mulung Preneur</h2>

<!-- GIFs showcase -->
<table width="100%">
  <tr>
    <!-- Top large gif -->
    <td colspan="2" align="center">
      <img src="https://github.com/wi1wil/MulungPreneur/raw/main/dog1.gif" width="100%"/>
    </td>
  </tr>
  <tr>
    <!-- Bottom two gifs -->
    <td align="center" width="50%">
      <img src="https://github.com/wi1wil/MulungPreneur/raw/main/dog2.gif" width="100%"/>
    </td>
    <td align="center" width="50%">
      <img src="https://github.com/wi1wil/MulungPreneur/raw/main/dog3.gif" width="100%"/>
    </td>
  </tr>
</table>

--- 

<!-- About the game -->
<table width="100%">
  <tr>
    <!-- Left Image -->
    <td width="30%" align="center" valign="middle" style="padding:15px;">
      <img src="https://github.com/wi1wil/wi1wil/raw/main/story.png" width="220"/>
    </td>
    <!-- Right Text -->
    <td width="70%" valign="top" style="padding:15px;">
      <h2>🌍 About </h2>
      <p style="max-width:700px;">
        Mulung Preneur is a serious game that raises awareness about pollution and promotes waste management. 
        Players step into the shoes of Adi, a passionate young man on a mission to reduce waste in his community. 
        By collecting, sorting, and recycling waste, Adi inspires others to take part in sustainable living and entrepreneurship.
      </p>
      <a href="https://wi1wil.itch.io/story-project">
        <img src="https://img.shields.io/badge/Itch.io-FA5C5C?style=for-the-badge&logo=itch.io&logoColor=white" />
      </a>
    </td>
  </tr>
</table>

<h2>📜 Scripts</h2>
<table>
  <tr>
    <th>Script</th>
    <th>Description</th>
  </tr>

  <tr>
    <td><code>GameManagerScript.cs</code></td>
    <td>Central hub that organizes and coordinates other managers, ensuring the game runs smoothly.</td>
  </tr>
  <tr>
    <td><code>PlayerSaveManagerScript.cs</code></td>
    <td>Handles saving and loading player progress, including stats, items, and quest data.</td>
  </tr>
  <tr>
    <td><code>InventoryManager.cs</code></td>
    <td>Controls the item system, including adding, removing, and organizing items in the inventory.</td>
  </tr>
  <tr>
    <td><code>QuestManager.cs</code></td>
    <td>Registers, tracks, and manages player quests and objectives.</td>
  </tr>
  <tr>
    <td><code>MapDualGridSystem.cs</code></td>
    <td>Manages the map’s dual grid structure for world navigation and object placement.</td>
  </tr>
</table>

---

<pre>
MulungPreneur                      # Root folder of the project
└── Assets                         # Default Unity folder for all game assets, scripts, and scenes
    ├── V1                         # Stores all assets and scripts for Version 1 of the game
    │   ├── _Animation             # Stores animation clips and controllers for V1
    │   ├── _Audio                 # Stores BGM and SFX audio clips for V1
    │   ├── _AudioMixers           # Stores audio mixer assets for volume control
    │   ├── _Fonts                 # Stores all fonts used in V1
    │   ├── _Imported              # Stores third-party assets used in V1
    │   ├── _Prefabs               # Stores pre-configured game objects for V1
    │   ├── _Quests                # Stores Scriptable Objects related to the quest system in V1
    │   ├── _Scenes                # Stores all Unity Scenes for V1
    │   ├── _Scripts               # Parent folder of all C# scripts used in V1
    │   │   ├── NPC                # Scripts related to Non-Player Character behavior (dialogue, shop, etc.)
    │   │   ├── Player             # Scripts related to player functionality (movement, data, currency)
    │   │   └── Quest              # Scripts related to the quest system (management, rewards, UI)
    │   ├── _Sprites               # Stores 2D sprites and UI elements for V1
    │   └── _Tilesets              # Stores tilesets used for creating tile-based maps in V1
    │
    └── V2                         # Stores all assets and scripts for Version 2 of the game
        ├── Animation              # Stores animation clips and controllers for V2
        ├── Scene                  # Stores all Unity Scenes for V2
        └── _Scripts               # Parent folder for all C# scripts used in V2
            ├── Cloud              # Scripts for procedural cloud generation and movement
            ├── Interface          # Contains C# interfaces for system contracts (e.g., IInteractable)
            ├── Inventory          # Scripts related to the player's inventory system (UI, slots, management)
            ├── Items              # Scripts for item behavior, data (ScriptableObjects), and interaction
            ├── Lighting           # Scripts for managing world time and lighting effects
            ├── Map                # Scripts for the map system, such as grid management
            ├── Menu               # Scripts for managing UI menus (main, pause, settings)
            ├── NPC                # Scripts for V2 Non-Player Character logic
            ├── Player             # Scripts for V2 player controls and data management
            └── Quest              # Scripts for the V2 questing system
</pre>

---

<h2>📋 Developers & Contributions</h2>
<table>
  <tr>
    <td align="center" width="120">
      <img src="https://github.com/wi1wil.png" width="80" style="border-radius:50%;" alt="Wilson"/>
    </td>
    <td align="left">
      <b>Wilson H.</b><br/>
      <sub>Lead Programmer</sub><br/>
      <p style="margin:0;">Implemented core systems, managers, and overall game logic.</p>
    </td>
  </tr>
  <tr>
    <td align="center" width="120">
      <img src="https://github.com/rchtr-chn.png" width="80" style="border-radius:50%;" alt="Richter"/>
    </td>
    <td align="left">
      <b>Richter C.</b><br/>
      <sub>2nd Programmer & 2nd Game Artist</sub><br/>
      <p style="margin:0;">Contributed additional programming features and secondary art assets.</p>
    </td>
  </tr>
  <tr>
    <td align="center" width="120">
      <img src="https://github.com/ChocoMOCC.png" width="80" style="border-radius:50%;" alt="Kelvin"/>
    </td>
    <td align="left">
      <b>Kelvin</b><br/>
      <sub>Lead Game Artist</sub><br/>
      <p style="margin:0;">Created the main visual assets, characters, and environments.</p>
    </td>
  </tr>
  <tr>
    <td align="center" width="120">
      <img src="https://github.com/kangmantul.png" width="80" style="border-radius:50%;" alt="Jordy"/>
    </td>
    <td align="left">
      <b>Jordy T.</b><br/>
      <sub>Lead Game Designer & Sound Designer</sub><br/>
      <p style="margin:0;">Designed gameplay mechanics and handled sound design.</p>
    </td>
  </tr>
  <tr>
    <td align="center" width="120">
      <img src="https://github.com/CallMeLynix.png" width="80" style="border-radius:50%;" alt="Andre"/>
    </td>
    <td align="left">
      <b>Andre J. L.</b><br/>
      <sub>2nd Game Designer</sub><br/>
      <p style="margin:0;">Assisted in game balance and level design.</p>
    </td>
  </tr>
</table>

---

<h2>🎮 Controls / Inputs</h2>

<table>
  <tr>
    <th>Action</th>
    <th>Key / Input</th>
  </tr>
  <tr>
    <td>Move Left</td>
    <td><b>A</b> / <b>←</b></td>
  </tr>
  <tr>
    <td>Move Right</td>
    <td><b>D</b> / <b>→</b></td>
  </tr>
  <tr>
    <td>Move Up</td>
    <td><b>W</b> / <b>↑</b></td>
  </tr>
  <tr>
    <td>Move Down</td>
    <td><b>S</b> / <b>↓</b></td>
  </tr>
  <tr>
    <td>Interact / Collect</td>
    <td><b>E</b></td>
  </tr>
  <tr>
    <td>Open Menu / Pause</td>
    <td><b>Esc</b></td>
  </tr>
  <tr>
    <td>Open Inventory </td>
    <td><b>Tab</b></td>
  </tr>
  <tr>
    <td>Confirm / Select</td>
    <td><b>Enter</b> / <b>Space</b></td>
  </tr>
</table>

<hr/>
