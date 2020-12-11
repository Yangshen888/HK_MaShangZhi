<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.process.query.totalCount"
        :pageSize="stores.process.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.process.query.kw"
                      placeholder="请输入菜品名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加"
                >添加</Button>
                <!-- <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportCuisine"
                  title="导入"
                >导入</Button>
                <Button
                  v-can="'export'"
                  icon="ios-cloud-download-outline"
                  type="primary"
                  @click="handleExportCuisine('export')"
                  title="导出"
                >导出</Button>-->
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
          :data="stores.process.data"
          :columns="stores.process.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <!-- <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" v-can="'delete'" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
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
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel.fields"
        ref="formdispatch"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="时间" prop="addTime">
              <DatePicker
                v-model="formModel.fields.addTime"
                :editable="false"
                type="date"
                format="yyyy-MM-dd"
                placeholder="选择时间"
                style="width: 150px"
                @on-change="toSearchCuisineList"
              ></DatePicker>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="类别" prop="mealType">
              <Select v-model="formModel.fields.mealType" @on-change="toSearchCuisineList">
                <Option
                  v-for="item in formModel.mealType"
                  :value="item.value"
                  :key="item.value"
                >{{item.text}}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="菜品名称" prop="cuisineUuid">
            <Select v-model="formModel.fields.cuisineUuid" @on-change="changeCuisineName">
              <Option
                v-for="item in formModel1.cuisines"
                :value="item.cuisineUuid"
                :key="item.cuisineUuid"
              >{{item.cuisineName}}</Option>
            </Select>
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="主料" prop="ingredient">
            <Input readonly v-model="formModel1.fields.ingredient" placeholder="请输入主料" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="配料" prop="burdening">
            <Input readonly v-model="formModel1.fields.burdening" placeholder="请输入配料" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="简介" prop="abstract">
            <Input readonly :autosize="true" v-model="formModel1.fields.abstracts" placeholder="请输入简介" type="textarea" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="买入" prop="people1">
            <Select
              v-model="formModel.fields.people1"
              filterable
              multiple
              @on-change="changePeople1"
            >
              <Option
                v-for="item in stores.staffList"
                :value="item.systemUserUuid"
                :key="item.systemUserUuid"
              >{{ item.realName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="买入时间" prop="buyingTime">
            <TimePicker
              v-model="formModel.fields.buyingTime"
              :editable="false"
              type="time"
              placeholder="选择时间"
              format="HH:mm:ss"
              style="width: 200px"
            ></TimePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="检测" prop="people2">
            <Select
              v-model="formModel.fields.people2"
              filterable
              multiple
              @on-change="changePeople2"
            >
              <Option
                v-for="item in stores.staffList"
                :value="item.systemUserUuid"
                :key="item.systemUserUuid"
              >{{ item.realName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="检测时间" prop="detectionTime">
            <TimePicker
              v-model="formModel.fields.detectionTime"
              :editable="false"
              type="time"
              placeholder="选择时间"
              format="HH:mm:ss"
              style="width: 200px"
            ></TimePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="洗菜" prop="people3">
            <Select
              v-model="formModel.fields.people3"
              filterable
              multiple
              @on-change="changePeople3"
            >
              <Option
                v-for="item in stores.staffList"
                :value="item.systemUserUuid"
                :key="item.systemUserUuid"
              >{{ item.realName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="洗菜时间" prop="washVegeTime">
            <TimePicker
              v-model="formModel.fields.washVegeTime"
              :editable="false"
              type="time"
              placeholder="选择时间"
              format="HH:mm:ss"
              style="width: 200px"
            ></TimePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="切配" prop="people4">
            <Select
              v-model="formModel.fields.people4"
              filterable
              multiple
              @on-change="changePeople4"
            >
              <Option
                v-for="item in stores.staffList"
                :value="item.systemUserUuid"
                :key="item.systemUserUuid"
              >{{ item.realName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="切配时间" prop="chopperTime">
            <TimePicker
              v-model="formModel.fields.chopperTime"
              :editable="false"
              type="time"
              placeholder="选择时间"
              format="HH:mm:ss"
              style="width: 200px"
            ></TimePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="成菜" prop="people5">
            <Select
              v-model="formModel.fields.people5"
              filterable
              multiple
              @on-change="changePeople5"
            >
              <Option
                v-for="item in stores.staffList"
                :value="item.systemUserUuid"
                :key="item.systemUserUuid"
              >{{ item.realName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="成菜时间" prop="cookTime">
            <TimePicker
              v-model="formModel.fields.cookTime"
              :editable="false"
              type="time"
              placeholder="选择时间"
              format="HH:mm:ss"
              style="width: 200px"
            ></TimePicker>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      :title="formTitle"
      v-model="formModel1.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
    >
      <Form :model="formModel1.fields" ref="formdispatch3" label-position="top">
        <Row :gutter="16">
          <Row :gutter="16">
            <Col span="12">
              <FormItem label="时间" prop="addTime">
                <DatePicker
                  readonly
                  v-model="formModel.fields.addTime"
                  type="date"
                  placeholder="选择时间"
                  style="width: 150px"
                ></DatePicker>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem label="类别" prop="mealType">
                <Select v-model="formModel.fields.mealType">
                  <Option
                    disabled
                    v-for="item in formModel.mealType"
                    :value="item.value"
                    :key="item.value"
                  >{{item.text}}</Option>
                </Select>
              </FormItem>
            </Col>
          </Row>
          <FormItem label="菜品名称" prop="cuisineUuid">
            <Select disabled v-model="formModel.fields.cuisineUuid" @on-change="changeCuisineName">
              <Option
                v-for="item in formModel1.cuisines"
                :value="item.cuisineUuid"
                :key="item.cuisineUuid"
              >{{item.cuisineName}}</Option>
            </Select>
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="主料" prop="ingredient">
            <Input readonly v-model="formModel1.fields.ingredient" placeholder="请输入主料" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="配料" prop="burdening">
            <Input readonly v-model="formModel1.fields.burdening" placeholder="请输入配料" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="简介" prop="abstract">
            <Input readonly :autosize="true" v-model="formModel1.fields.abstracts" placeholder="请输入简介" type="textarea" />
          </FormItem>
        </Row>

        <Row :gutter="16">
          <FormItem label="买入" prop="people1">
            <Select
              disabled
              v-model="formModel.fields.people1"
              filterable
              multiple
              @on-change="changePeople1"
            >
              <Option
                v-for="item in stores.staffList"
                :value="item.systemUserUuid"
                :key="item.systemUserUuid"
              >{{ item.realName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="买入时间" prop="buyingTime">
            <TimePicker
              readonly
              v-model="formModel.fields.buyingTime"
              type="time"
              placeholder="选择时间"
              format="HH:mm:ss"
              style="width: 200px"
            ></TimePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="检测" prop="people2">
            <Select
              disabled
              v-model="formModel.fields.people2"
              filterable
              multiple
              @on-change="changePeople2"
            >
              <Option
                v-for="item in stores.staffList"
                :value="item.systemUserUuid"
                :key="item.systemUserUuid"
              >{{ item.realName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="检测时间" prop="detectionTime">
            <TimePicker
              readonly
              v-model="formModel.fields.detectionTime"
              type="time"
              placeholder="选择时间"
              format="HH:mm:ss"
              style="width: 200px"
            ></TimePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="洗菜" prop="people3">
            <Select
              disabled
              v-model="formModel.fields.people3"
              filterable
              multiple
              @on-change="changePeople3"
            >
              <Option
                v-for="item in stores.staffList"
                :value="item.systemUserUuid"
                :key="item.systemUserUuid"
              >{{ item.realName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="洗菜时间" prop="washVegeTime">
            <TimePicker
              readonly
              v-model="formModel.fields.washVegeTime"
              type="time"
              placeholder="选择时间"
              format="HH:mm:ss"
              style="width: 200px"
            ></TimePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="切配" prop="people4">
            <Select
              disabled
              v-model="formModel.fields.people4"
              filterable
              multiple
              @on-change="changePeople4"
            >
              <Option
                v-for="item in stores.staffList"
                :value="item.systemUserUuid"
                :key="item.systemUserUuid"
              >{{ item.realName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="切配时间" prop="chopperTime">
            <TimePicker
              readonly
              v-model="formModel.fields.chopperTime"
              type="time"
              placeholder="选择时间"
              format="HH:mm:ss"
              style="width: 200px"
            ></TimePicker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="成菜" prop="people5">
            <Select
              disabled
              v-model="formModel.fields.people5"
              filterable
              multiple
              @on-change="changePeople5"
            >
              <Option
                v-for="item in stores.staffList"
                :value="item.systemUserUuid"
                :key="item.systemUserUuid"
              >{{ item.realName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="成菜时间" prop="cookTime">
            <TimePicker
              readonly
              v-model="formModel.fields.cookTime"
              type="time"
              placeholder="选择时间"
              format="HH:mm:ss"
              style="width: 200px"
            ></TimePicker>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button style="margin-left: 30px" icon="md-close" @click="formModel1.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getProcessList,
  createProcess,
  loadProcess,
  editProcess,
  deleteProcess,
  batchCommand
} from "@/api/process/process";
import { cuisineSelectList } from "@/api/diningRoom/liveShot";
import { getStaffList } from "@/api/rbac/user";
import { DateToString } from "@/global/utils";
// import { cuisineSelectList } from "@/api/cuisine/cuisine";
import config from "@/config";
import {
  globalvalidateIDCardNoMust,
  globalvalidatepwd,
  globalvalidateIsNotEmpty
} from "@/global/validate";
export default {
  name: "process",
  components: {
    DzTable
  },
  data() {
    return {
      importdisable: false,

      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false
      },

      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        Import: { name: "Import", title: "导入" },
        export: { name: "export", title: "导出" }
      },
      stores: {
        process: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {},
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "菜品名称", key: "cuisineName", sortable: true },
            { title: "主料", key: "ingredient",ellipsis:true },
            { title: "配料", key: "burdening",ellipsis:true },
            { title: "简介", key: "abstract",ellipsis:true },
            { title: "用餐类别", key: "mealType" },
            { title: "时间", key: "addTime" },
            { title: "已完成状态", key: "state" },
            {
              title: "操作",
              align: "center",
              width: 200,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        },
        staffList: []
      },
      schoolshow: 0,
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        people1: [],
        people2: [],
        people3: [],
        people4: [],
        people5: [],
        fields: {
          people1: [],
          people2: [],
          people3: [],
          people4: [],
          people5: [],
          mealFlowUuid: "",
          cuisineUuid: "",
          mealType: "",
          buying: "",
          detection: "",
          washVege: "",
          chopper: "",
          cook: "",
          addTime: "",
          isDelete: "",
          schoolUUID: "",
          buyingTime: "",
          detectionTime: "",
          washVegeTime: "",
          chopperTime: "",
          cookTime: "",
          schoolName: ""
        },
        schooltype: [{ schoolUuid: "0", schoolName: "请选择" }],
        mealType: [
          { value: "morn", text: "早餐" },
          { value: "noon", text: "午餐" },
          { value: "night", text: "晚餐" }
        ],
        rules: {
          cuisineUuid: [
            { required: true, message: "请选择菜品", trigger: "change" }
          ],
          mealType: [
            { required: true, message: "请选择类型", trigger: "change" }
          ],
          addTime: [
            {
              type: "date",
              required: true,
              message: "选择时间",
              trigger: "change"
            }
          ]
          // buyingTime: [
          //   {
          //     type: "string",
          //     required: true,
          //     message: "选择时间",
          //     trigger: "change"
          //   }
          // ],
          // detectionTime: [
          //   {
          //     type: "string",
          //     required: true,
          //     message: "选择时间",
          //     trigger: "change"
          //   }
          // ],
          // washVegeTime: [
          //   {
          //     type: "string",
          //     required: true,
          //     message: "选择时间",
          //     trigger: "change"
          //   }
          // ],
          // chopperTime: [
          //   {
          //     type: "string",
          //     required: true,
          //     message: "选择时间",
          //     trigger: "change"
          //   }
          // ],
          // cookTime: [
          //   {
          //     type: "string",
          //     required: true,
          //     message: "选择时间",
          //     trigger: "change"
          //   }
          // ],
          // people1: [{ required: true, message: "请选择人员" }],
          // people2: [{ required: true, message: "请选择人员" }],
          // people3: [{ required: true, message: "请选择人员" }],
          // people4: [{ required: true, message: "请选择人员" }],
          // people5: [{ required: true, message: "请选择人员" }]
        }
      },
      formModel1: {
        opened: false,
        selection: [],
        people1: [],
        people2: [],
        people3: [],
        people4: [],
        people5: [],
        cuisines: [],
        fields: {
          processName: "",
          price: "",
          burdening: "",
          ingredient: "",
          abstract: "",
          processType: "",
          isDeleted: 0,
          accessory: "",
          schoolUuid: "",
          addPeople: "",
          addTime: "",
          schoolName: "",
          buyingTime: "",
          detectionTime: "",
          washVegeTime: "",
          chopperTime: "",
          cookTime: ""
        }
      }
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "新增成菜流程信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑成菜流程信息";
      }
      if (this.formModel.mode === "show") {
        let s = "查看成菜流程信息";
        if (
          this.formModel.fields.schoolName != "" &&
          this.$store.state.user.schoolguid == null
        ) {
          s += "   学校:(" + this.formModel.fields.schoolName + ")";
        }

        return s;
        // return "查看成菜流程信息";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.mealFlowUuid);
    } //删除
  },
  methods: {
    //页面加载
    loadDispatchList() {
      getProcessList(this.stores.process.query).then(res => {
        console.log(res.data.data);
        this.stores.process.data = res.data.data;
        this.stores.process.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    //多选
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.process.query.currentPage = page;
      this.loadDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.process.query.pageSize = pageSize;
      this.loadDispatchList();
    },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //搜索
    handleSearchDispatch() {
      this.loadDispatchList();
    },
    //刷新
    handleRefresh() {
      this.loadDispatchList();
    },
    //清空
    handleResetFormDispatch() {
      if (this.formModel1.mode == "show") {
        this.$refs["formdispatch3"].resetFields();
      } else {
        this.$refs["formdispatch"].resetFields();
      }
      this.formModel.fields.cuisineUuid="";
      this.formModel.fields.processName = "";
      this.formModel.fields.price = "";
      this.formModel.fields.schoolUuid = "";
      this.formModel.fields.addTime = "";
      this.formModel.fields.cuisineUuid = "";
      this.formModel1.fields.ingredient = "";
      this.formModel1.cuisines=[];
    },
    //右边删除（单个删除）
    handleDelete(row) {
      console.log(row);
      this.doDelete(row.mealFlowUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteProcess(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //右上边删除按钮
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        }
      });
    },
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },

    //学校选择
    changeschool(e) {
      this.formModel.fields.schoolUuid = e;
    },
    //类别选择
    // changetype(e) {
    //   this.formModel.fields.processType = e;
    // },
    //添加按钮
    handleShowCreateWindow() {
      this.formModel.people1 = [];
      this.formModel.people2 = [];
      this.formModel.people3 = [];
      this.formModel.people4 = [];
      this.formModel.people5 = [];
      this.formModel.fields.mealType = "";
      this.formModel.fields.ingredient = "";
      this.formModel1.fields.burdening = "";
      this.formModel1.fields.abstracts = "";
      let date = new Date();
      this.formModel.fields.addTime = "";
      // this.formModel.fields.addTime =
      date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
      console.log(this.formModel.fields.addTime);
      this.formModel.mode = "create";
      if (this.stores.process.query.kw == "") {
        this.schoolshow = 1;
      }
      this.handleResetFormDispatch(); //清空表单
      this.loadStaffList();
      // this.loadCuiSineList(null, "morn");
      this.formModel.opened = true;
      // this.formModel.fields.addTime = DateToString(new Date());
      this.formModel.fields.mealType = "morn";
    },
    //右边编辑
    async handleEdit(row) {
      console.log(row);
      this.formModel.mode = "edit";
      if (this.stores.process.query.kw == "") {
        this.schoolshow = 1;
      }
      await this.loadStaffList();
      // await this.loadCuiSineList();
      this.formModel.opened = true;
      this.doLoadData(row.mealFlowUuid);
      this.handleResetFormDispatch(); //清空表单
    },
    //右边详情
    async handleDetail(row) {
      this.formModel.mode = "show";
      this.formModel1.opened = true;
      if (this.stores.process.query.kw == "") {
        this.schoolshow = 1;
      }
      // await this.loadCuiSineList();
      await this.loadStaffList();
      this.doLoadData(row.mealFlowUuid);
      this.handleResetFormDispatch(); //清空表单
    },
    //查询当前行信息
    async doLoadData(id) {
      await loadProcess({ guid: id }).then(res => {
        if (res.data.code === 200) {
          console.log(res.data.data);

          this.formModel.fields = res.data.data;
          if (this.formModel.mode != "create") {
            console.log(res.data.data.addTime);
            this.loadCuiSineList2(
              res.data.data.addTime,
              res.data.data.mealType
            );
          }
          this.formModel.fields.people1 = res.data.data.buying.split(",");
          this.formModel.fields.people2 = res.data.data.detection.split(",");
          this.formModel.fields.people3 = res.data.data.washVege.split(",");
          this.formModel.fields.people4 = res.data.data.chopper.split(",");
          this.formModel.fields.people5 = res.data.data.cook.split(",");
          console.log(this.formModel.fields);
          console.log(this.stores.staffList);
        }
      });
    },
    //保存按钮
    handleSubmitConsumable() {
      console.log(this.formModel1.fields.addTime);
      let valid = this.validateUserForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.docreateDispatch();
        }
        if (this.formModel.mode === "edit") {
          this.doEditDispatch();
        }
      }
      // else {
      //   this.$Message.error("请完善表单信息");
      // }
    },
    validateUserForm() {
      let _valid = false;
      this.$refs["formdispatch"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
          return _valid;
        } else {
          _valid = true;
          // this.$refs["formdispatch2"].validate(valid2 => {
          //   if (!valid2) {
          //     this.$Message.error("请完善表单信息");
          //     return _valid;
          //   } else {
          //     _valid = true;
          //   }
          // });
        }
      });
      console.log(_valid);
      return _valid;
    },
    //添加（保存）
    docreateDispatch() {
      //this.formModel.fields.addPeople = this.$store.state.user.userName;
      createProcess(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑（保存）
    doEditDispatch() {
      editProcess(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },

    async loadStaffList() {
      await getStaffList().then(res => {
        console.log(res);
        this.stores.staffList = res.data.data;
      });
    },
    // async loadCuiSineList() {
    //   await cuisineSelectList().then(res => {
    //     console.log(res);
    //     this.formModel1.cuisines = res.data.data;
    //   });
    // },
    async loadCuiSineList(time, type) {
      console.log(22222);
      console.log(time, type);
      let date = DateToString(time);
      await cuisineSelectList({ date: date, type: type }).then(res => {
        console.log(res);
        this.formModel1.cuisines = res.data.data;
      });
    },
    async loadCuiSineList2(time, type) {
      console.log(333333);
      await cuisineSelectList({ date: time, type: type }).then(res => {
        console.log(res);
        this.formModel1.cuisines = res.data.data;
      });
      await this.setInfo();
    },
    changePeople1(e) {
      console.log(e);
      this.formModel.fields.buying = e.join();
    },
    changePeople2(e) {
      console.log(e);
      this.formModel.fields.detection = e.join();
    },
    changePeople3(e) {
      console.log(e);
      this.formModel.fields.washVege = e.join();
    },
    changePeople4(e) {
      console.log(e);
      this.formModel.fields.chopper = e.join();
    },
    changePeople5(e) {
      console.log(e);
      this.formModel.fields.cook = e.join();
    },
    changeCuisineName(e) {
      console.log(e);
      if (e == null || e == undefined) {
        return;
      }
      let data = this.formModel1.cuisines.find(x => x.cuisineUuid == e);
      this.formModel1.fields.ingredient = data.ingredient;
      this.formModel1.fields.burdening = data.burdening;
      this.formModel1.fields.abstracts = data.abstract;
    },
    async setInfo() {
      console.log(this.formModel1.cuisines);
      console.log(this.formModel.fields.cuisineUuid);
      let data = await this.formModel1.cuisines.find(
        x => x.cuisineUuid == this.formModel.fields.cuisineUuid
      );
      console.log(1);
      console.log(data);
      this.formModel1.fields.ingredient = data.ingredient;
      this.formModel1.fields.burdening = data.burdening;
      this.formModel1.fields.abstracts = data.abstract;
    },
    toSearchCuisineList() {
      console.log(this.formModel.fields.addTime);
      // let date = DateToString(this.formModel.fields.addTime);
      let date = DateToString(new Date(this.formModel.fields.addTime));
      console.log(date);
      cuisineSelectList({
        date: date,
        type: this.formModel.fields.mealType
      }).then(res => {
        console.log(res);
        this.formModel1.cuisines = res.data.data;
      });
      this.formModel.fields.cuisineUuid = "";
      this.formModel1.fields.abstracts = "";
      this.formModel1.fields.burdening = "";
      this.formModel1.fields.ingredient = "";
    }
  },
  mounted() {
    this.loadDispatchList(); //页面加载
  },
  created() {
    if (this.$store.state.user.schoolguid == null) {
      this.stores.process.columns.splice(1, 0, {
        title: "学校",
        key: "schoolName"
      });
    }
  }
};
</script>
<style>
.file1:hover + .fileimg1:hover {
  transform: scale(1.01, 1.01);
  box-shadow: 0 0 3px #1783ba;
}
.fileimg1 {
  width: 300px;
  height: 169px;
  float: left;
  z-index: 2;
}
</style>