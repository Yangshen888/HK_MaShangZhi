<template>
  <div>
    <Card>
      <dz-table :totalCount="stores.menu.query.totalCount">
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem v-show="schoollist==1">
                    <Select
                      v-model="formModel.fields.schoolUuid"
                      style="width:200px"
                      @on-change="xz"
                      clearable
                      placeholder="请选择学校"
                    >
                      <Option
                        v-for="item in this.school"
                        :value="item.schoolUuid"
                        :key="item.schoolUuid"
                      >{{ item.schoolName }}</Option>
                    </Select>
                  </FormItem>
                </Form>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.menu.data"
          :columns="stores.menu.columns"
        >
          <template slot-scope="{ row, index }" slot="action">
            <!-- <Tooltip placement="top" content="添加" :delay="1000" v-show="schoolshow==0" :transfer="true">
              <Button
                v-can="'create'"
                type="success"
                size="small"
                shape="circle"
                icon="ios-create-outline"
                @click="handleCreate(row)"
              ></Button>
            </Tooltip> -->
            <Tooltip placement="top" content="编辑" :delay="1000" v-show="schoolshow==0" v-if="index!=0" :transfer="true">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" v-show="schoolshow1==1"  :transfer="true">
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="1000"
      :mask-closable="false"
      :mask="true"
    >
      <Form :model="formModel.fields" ref="formdispatch" label-position="left">
        <table width="100%" class="menutable">
          <tr class="tablehead">
            <th>星期</th>
            <th>早餐</th>
            <th>午餐</th>
            <th>晚餐</th>
          </tr>
          <tr>
            <td>星期一</td>
            <td>
              <Select
                v-model="model1"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td>
              <Select
                v-model="model2"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td class="tdright">
              <Select
                v-model="model3"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
          </tr>
          <tr>
            <td>星期二</td>
            <td>
              <Select
                v-model="model4"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td>
              <Select
                v-model="model5"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td class="tdright">
              <Select
                v-model="model6"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
          </tr>
          <tr>
            <td>星期三</td>
            <td>
              <Select
                v-model="model7"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td>
              <Select
                v-model="model8"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td class="tdright">
              <Select
                v-model="model9"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
          </tr>
          <tr>
            <td>星期四</td>
            <td>
              <Select
                v-model="model10"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td>
              <Select
                v-model="model11"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td class="tdright">
              <Select
                v-model="model12"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
          </tr>
          <tr>
            <td>星期五</td>
            <td>
              <Select
                v-model="model13"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td>
              <Select
                v-model="model14"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td class="tdright">
              <Select
                v-model="model15"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
          </tr>

          <tr>
            <td>星期六</td>
            <td>
              <Select
                v-model="model16"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td>
              <Select
                v-model="model17"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td class="tdright">
              <Select
                v-model="model18"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
          </tr>
          <tr>
            <td>星期日</td>
            <td>
              <Select
                v-model="model19"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td>
              <Select
                v-model="model20"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
            <td class="tdright">
              <Select
                v-model="model21"
                multiple
                style="width: 238px;"
                :disabled="disable()"
                filterable
              >
                <Option
                  v-for="item in cuiList"
                  :key="item.cuisineUuid"
                  :value="item.cuisineUuid"
                >{{ item.cuisineName }}</Option>
              </Select>
            </td>
          </tr>
        </table>
      </Form>
      <div class="demo-drawer-footer">
        <!-- <Button icon="md-checkmark-circle" type="primary" @click="handleSubmit" v-show="baocunbtn==1">保 存</Button> -->
        <Button icon="md-checkmark-circle"  type="primary" :loading="loading" @click="handleSubmit" v-show="baocunbtn==1">
            <span v-if="!loading">保 存</span>
            <span v-else>保 存</span>
        </Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  time,
  cuisineList,
  WeekMenuCreate,
  Getweekmenu,
  WeekMenuEdit,
  SchoolList
} from "@/api/weekmenu/weekmenu";
export default {
  name: "weekmenu",
  components: {
    DzTable
  },
  data() {
    return {
      loading: false,
      cuiList: [],
      model1: [],
      model2: [],
      model3: [],
      model4: [],
      model5: [],
      model6: [],
      model7: [],
      model8: [],
      model9: [],
      model10: [],
      model11: [],
      model12: [],
      model13: [],
      model14: [],
      model15: [],
      model16: [],
      model17: [],
      model18: [],
      model19: [],
      model20: [],
      model21: [],
      stores: {
        menu: {
          query: {
            totalCount: 3
          },
          //列表显示
          columns: [
            { title: "序号", type: "index", width: 80, align: "center" },
            { title: "菜单", key: "weekmenu", align: "center" },
            { title: "时间", key: "time", align: "center" },
            {
              title: "操作",
              align: "center",
              width: 200,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: [
            {
              weekmenu: "上周菜单",
              time: ""
            },
            {
              weekmenu: "本周菜单",
              time: ""
            },
            {
              weekmenu: "下周菜单",
              time: ""
            }
          ]
        }
      },
      row: {},
      schoolshow:-1,
      schoolshow1:-1,
      schoollist:1,
      school:[],
      showdis: 0,
      baocunbtn:1,
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          monMon: "",
          monNoon: "",
          monNight: "",
          tuesMon: "",
          tuesNoon: "",
          tuesNight: "",
          wedMon: "",
          wedNoon: "",
          wedNight: "",
          thursMon: "",
          thursNoon: "",
          thursNight: "",
          friMon: "",
          friNoon: "",
          friNight: "",
          satMon: "",
          satNoon: "",
          satNight: "",
          sunMon: "",
          sunNoon: "",
          sunNight: "",
          schoolUuid: "",
          addPeople: "",
          week: "",
          time: "",
          iscunzai:0
        }
      }
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        this.baocunbtn=1;
        return "新增菜单信息";
      }
      if (this.formModel.mode === "edit") {
        this.baocunbtn=1;
        return "编辑菜单信息";
      }
      if (this.formModel.mode === "view") {
        this.baocunbtn=0
        return "查看菜单信息";
      }
      return "";
    }
  },
  methods: {
    //页面加载
    loadDispatchList() {
      console.log(this.formModel.fields.schoolUuid)
      if(this.$store.state.user.schoolguid!=null){
        this.schoolshow=0;
        this.schoolshow1=1;
        this.schoollist=0;
        this.formModel.fields.schoolUuid=this.$store.state.user.schoolguid;
      }else if(this.formModel.fields.schoolUuid=="" ||this.formModel.fields.schoolUuid==undefined){
        this.schoollist=1;
        this.schoolshow1=-1;
      }else{
        this.schoolshow1=1;
      }
      time().then(res => {
        this.stores.menu.data[0].time = res.data.data.sdate;
        this.stores.menu.data[1].time = res.data.data.tdate;
        this.stores.menu.data[2].time = res.data.data.ndate;
      });
    },
    //菜品下拉框
    doCuisineList() {
      cuisineList(this.$store.state.user.schoolguid).then(res => {
        this.cuiList = res.data.data;
      });
    },
    //添加
    handleCreate(row) {
      this.formModel.mode = "create";
      this.showdis = 0;
      this.row = row;
      this.formModel.opened = true;
      this.empty();
      this.doCuisineList();
    },
    changesss(row) {
      this.formModel.fields.monMon = this.model1.join(",");
      this.formModel.fields.monNoon = this.model2.join(",");
      this.formModel.fields.monNight = this.model3.join(",");
      this.formModel.fields.tuesMon = this.model4.join(",");
      this.formModel.fields.tuesNoon = this.model5.join(",");
      this.formModel.fields.tuesNight = this.model6.join(",");
      this.formModel.fields.wedMon = this.model7.join(",");
      this.formModel.fields.wedNoon = this.model8.join(",");
      this.formModel.fields.wedNight = this.model9.join(",");
      this.formModel.fields.thursMon = this.model10.join(",");
      this.formModel.fields.thursNoon = this.model11.join(",");
      this.formModel.fields.thursNight = this.model12.join(",");
      this.formModel.fields.friMon = this.model13.join(",");
      this.formModel.fields.friNoon = this.model14.join(",");
      this.formModel.fields.friNight = this.model15.join(",");
      this.formModel.fields.satMon = this.model16.join(",");
      this.formModel.fields.satNoon = this.model17.join(",");
      this.formModel.fields.satNight = this.model18.join(",");
      this.formModel.fields.sunMon = this.model19.join(",");
      this.formModel.fields.sunNoon = this.model20.join(",");
      this.formModel.fields.sunNight = this.model21.join(",");
      this.formModel.fields.schoolUuid = this.$store.state.user.schoolguid;
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      this.formModel.fields.week = row.weekmenu;
      this.formModel.fields.time = row.time;
    },
    //保存
    handleSubmit() {
      let row = this.row;
      this.changesss(row);
      this.loading = true;
      if (this.formModel.mode === "create") {
        this.doCreateSubmit();
      }
      if (this.formModel.mode === "edit") {
        this.doEditDispatch();
      }
    },
    //添加保存
    doCreateSubmit() {
      WeekMenuCreate(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.empty();
          this.loading = false;
        } else {
          this.$Message.warning(res.data.message);
          this.loading = false;
        }
      });
    },
    //清空
    empty() {
      this.formModel.fields.monMon = "";
      this.formModel.fields.monNoon = "";
      this.formModel.fields.monNight = "";
      this.formModel.fields.tuesMon = "";
      this.formModel.fields.tuesNoon = "";
      this.formModel.fields.tuesNight = "";
      this.formModel.fields.wedMon = "";
      this.formModel.fields.wedNoon = "";
      this.formModel.fields.wedNight = "";
      this.formModel.fields.thursMon = "";
      this.formModel.fields.thursNoon = "";
      this.formModel.fields.thursNight = "";
      this.formModel.fields.friMon = "";
      this.formModel.fields.friNoon = "";
      this.formModel.fields.friNight = "";
      this.formModel.fields.satMon = "";
      this.formModel.fields.satNoon = "";
      this.formModel.fields.satNight = "";
      this.formModel.fields.sunMon = "";
      this.formModel.fields.sunNoon = "";
      this.formModel.fields.sunNight = "";
      // this.formModel.fields.schoolUuid = "";
      this.formModel.fields.addPeople = "";
      this.formModel.fields.week = "";
      this.formModel.fields.time = "";
      this.model1 = [];
      this.model2 = [];
      this.model3 = [];
      this.model4 = [];
      this.model5 = [];
      this.model6 = [];
      this.model7 = [];
      this.model8 = [];
      this.model9 = [];
      this.model10 = [];
      this.model11 = [];
      this.model12 = [];
      this.model13 = [];
      this.model14 = [];
      this.model15 = [];
      this.model16 = [];
      this.model17 = [];
      this.model18 = [];
      this.model19 = [];
      this.model20 = [];
      this.model21 = [];
    },
    doEditDispatch() {
        WeekMenuEdit(this.formModel.fields).then(res => {
          if (res.data.code === 200) {
            if(res.data.data==1){
              this.formModel.fields.iscunzai=1;
              this.$Message.warning("请确认您的菜品清空操作");
              this.loading = false;
              return;
            }else{
              console.log(res.data);
              this.$Message.success(res.data.message);
              this.formModel.opened = false; //关闭表单
              this.empty();
              this.loading = false;
            }
          } else {
            this.$Message.warning(res.data.message);
              this.loading = false;
          }
      });
    },
    //编辑
    handleEdit(row) {
      this.showdis = 0;
      this.formModel.mode = "edit";
      this.row = row;
      this.formModel.opened = true;
      this.empty();
      this.doCuisineList();
      this.doLoadData(row);
    },
    doSchoolList() {
      SchoolList().then(res => {
        this.school=res.data.data;
      });
    },
    xz(e){
        this.formModel.fields.schoolUuid=e;
      this.loadDispatchList();
      console.log(this.formModel.fields.schoolUuid)
    },
    disable() {
      if (this.formModel.mode == "view") {
        return true;
      } else {
        return false;
      }
    },
    //详情
    handleDetail(row) {
      this.showdis = 1;
      this.formModel.mode = "view";
      this.row = row;
      this.formModel.opened = true;
      this.doCuisineList();
      this.doLoadData(row);
      this.empty();
    },
    //查询当前
    doLoadData(row) {
      let data = {
        time: row.time,
        guid: this.formModel.fields.schoolUuid
      };
      Getweekmenu(data).then(res => {
        if (res.data.data == null) {
          this.$Message.warning("暂无数据");
          return;
        }
        this.model1 = res.data.data.monMon.split(",");
        this.model2 = res.data.data.monNoon.split(",");
        this.model3 = res.data.data.monNight.split(",");
        this.model4 = res.data.data.tuesMon.split(",");
        this.model5 = res.data.data.tuesNoon.split(",");
        this.model6 = res.data.data.tuesNight.split(",");
        this.model7 = res.data.data.wedMon.split(",");
        this.model8 = res.data.data.wedNoon.split(",");
        this.model9 = res.data.data.wedNight.split(",");
        this.model10 = res.data.data.thursMon.split(",");
        this.model11 = res.data.data.thursNoon.split(",");
        this.model12 = res.data.data.thursNight.split(",");
        this.model13 = res.data.data.friMon.split(",");
        this.model14 = res.data.data.friNoon.split(",");
        this.model15 = res.data.data.friNight.split(",");
        this.model16 = res.data.data.satMon.split(",");
        this.model17 = res.data.data.satNoon.split(",");
        this.model18 = res.data.data.satNight.split(",");
        this.model19 = res.data.data.sunMon.split(",");
        this.model20 = res.data.data.sunNoon.split(",");
        this.model21 = res.data.data.sunNight.split(",");
      });
    }
  },
  mounted() {
    this.loadDispatchList(); //页面加载
    this.doSchoolList();
  }
};
</script>
<style>
.menutable {
  font-size: 14px;
  table-layout: fixed;
}
.menutable .tablehead {
  border-color: inherit;
}
.menutable tr th {
  overflow: hidden;
  background-color: #f5f7f9;
  height: 48px;
  text-align: center;
  border-bottom: 1px solid #e3e8ee;
}
.menutable tr td {
  border-left: 1px solid #d7dde4;
  border-bottom: 1px solid #d7dde4;
  height: 100px;
  text-align: center;
  overflow: hidden;
}
.menutable .tdright {
  border-right: 1px solid #d7dde4;
}
</style>